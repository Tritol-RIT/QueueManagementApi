using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Application.Extensions;
using QueueManagementApi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextCustom(builder.Configuration);
builder.Services.AddServices(builder.Configuration); // add services from application layer
builder.Services.AddRepositories();

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetService<QueueManagementDbContext>();
    await dbContext!.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();