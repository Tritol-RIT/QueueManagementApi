using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using QueueManagementApi.Application.Models;
using QueueManagementApi.Application.Services.AuthService;
using QueueManagementApi.Application.Services.CategoryService;
using QueueManagementApi.Application.Services.EmailService;
using QueueManagementApi.Application.Services.EncryptionService;
using QueueManagementApi.Application.Services.ExhibitService;
using QueueManagementApi.Application.Services.QrCodeService;
using QueueManagementApi.Application.Services.SetPasswordTokenService;
using QueueManagementApi.Application.Services.TokenService;
using QueueManagementApi.Application.Services.UserService;
using QueueManagementApi.Application.Services.VisitorService;
using QueueManagementApi.Application.Services.WaitTimeCalculationService;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;
using QueueManagementApi.Core.Services.WaitTimeCalculationService;
using QueueManagementApi.Infrastructure.Data;
using QueueManagementApi.Infrastructure.Repositories;
using QueueManagementApi.Infrastructure.Services.FileService;
using QueueManagementApi.Infrastructure.UnitOfWork;
using System.Text;

namespace QueueManagementApi.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddDbContextCustom(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbContext, QueueManagementDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres"), npgsqloptions => { }));
    }

    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IExhibitService, ExhibitService>();

        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISetPasswordTokenService, SetPasswordTokenService>();


        services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));

        services.Configure<TokenSettings>(x => configuration.GetSection("TokenSettings").Get<TokenSettings>());

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailTemplateRenderer>();
        services.AddScoped<EmailService>();

        services.AddScoped<IFileService, LocalFileService>();

        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<IVisitorService, VisitorService>();
        services.AddScoped<IWaitTimeCalculationService, WaitTimeCalculationService>();
        services.AddScoped<IQrCodeService, QrCodeService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Exhibit>, GenericRepository<Exhibit>>();
        services.AddScoped<IRepository<User>, GenericRepository<User>>();

        services.AddScoped<IRepository<Visit>, GenericRepository<Visit>>();
        services.AddScoped<IRepository<Visitor>, GenericRepository<Visitor>>();
        services.AddScoped<IRepository<Group>, GenericRepository<Group>>();

        services.AddScoped<IVisitRepository, VisitRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Use IOptions to retrieve TokenSettings configured in ConfigureServices
        var tokenSettings = configuration.GetSection("TokenSettings").Get<TokenSettings>();

        if (tokenSettings == null)
            throw new InvalidOperationException("Token Settings must be configured");

        var key = Encoding.ASCII.GetBytes(tokenSettings.Secret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            x.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var tokenClaims = context.Principal?.Claims;
                    var tokenType = tokenClaims?.FirstOrDefault(c => c.Type == "tokenType")?.Value;

                    if (tokenType == "refresh")
                        context.Fail("Refresh token used in access token context.");
                    
                    return Task.CompletedTask;
                }
            };
        });
    }
}