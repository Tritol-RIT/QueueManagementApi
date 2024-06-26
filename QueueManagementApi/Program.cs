using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using QueueManagementApi.Application.Extensions;
using QueueManagementApi.Application.Services.EncryptionService;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Enums;
using QueueManagementApi.Core.Interfaces;
using QueueManagementApi.Infrastructure.Data;
using QueueManagementApi.JsonConverters;
using QueueManagementApi.Middlewares;
using QueueManagementApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
{
    // Add the PagedListConverter to the converters list
    options.SerializerSettings.Converters.Add(new PagedListConverter());
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Define the OAuth2.0 scheme that's in use (i.e., Implicit, Password, etc.)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

              },
              new List<string>()
            }
        });
});

builder.Services.AddDbContextCustom(builder.Configuration);
builder.Services.AddServices(builder.Configuration); // add services from application layer
builder.Services.AddRepositories();

builder.Services.AddFluentValidation(x =>
    x.RegisterValidatorsFromAssemblyContaining<CreateUserDtoValidator>());

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetService<QueueManagementDbContext>();
    await dbContext!.Database.MigrateAsync();

    var userRepo = scope.ServiceProvider.GetService<IRepository<User>>();
    var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
    var encryptionService = scope.ServiceProvider.GetService<IEncryptionService>();

    var userSeedSettings = builder.Configuration.GetSection("SeedAdminUserData");
    var adminName= userSeedSettings.GetValue<string>("Name");
    var adminSurname = userSeedSettings.GetValue<string>("Surname");
    var adminEmail = userSeedSettings.GetValue<string>("AdminEmail");
    var adminPassword = userSeedSettings.GetValue<string>("AdminPassword");

    if (!await userRepo!.FindByCondition(x => x.Email == adminEmail).AnyAsync())
    {
        var newUser = new User
        {
            Email = adminEmail!,
            FirstName = adminName!,
            LastName = adminSurname!,
            PasswordHash = encryptionService!.HashPassword(adminPassword!),
            Role = UserRole.Admin,
        };

        await userRepo!.AddAsync(newUser);
        await unitOfWork!.CompleteAsync();
    }
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(x => x
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

var filesDirectoryName = Path.Combine(app.Environment.ContentRootPath, "Files");

if (!Directory.Exists(filesDirectoryName))
    Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "Files"));

// Additional configuration to serve files from a custom folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(filesDirectoryName),
    RequestPath = "/api/files"
});

app.MapControllers();

app.Run();