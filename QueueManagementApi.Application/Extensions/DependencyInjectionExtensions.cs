using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using QueueManagementApi.Application.Models;
using QueueManagementApi.Application.Services.AuthService;
using QueueManagementApi.Application.Services.EncryptionService;
using QueueManagementApi.Application.Services.TokenService;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;
using QueueManagementApi.Infrastructure.Data;
using QueueManagementApi.Infrastructure.Repositories;
using QueueManagementApi.Infrastructure.UnitOfWork;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.EmailService;
using QueueManagementApi.Application.Services.ExhibitService;
using QueueManagementApi.Application.Services.SetPasswordTokenService;
using QueueManagementApi.Infrastructure.Services.FileService;

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
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISetPasswordTokenService, SetPasswordTokenService>();
        

        services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));

        services.Configure<TokenSettings>(x => configuration.GetSection("TokenSettings").Get<TokenSettings>());

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailTemplateRenderer>();
        services.AddScoped<EmailService>();

        services.AddScoped<IFileService, LocalFileService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Exhibit>, GenericRepository<Exhibit>>();
        services.AddScoped<IRepository<User>, GenericRepository<User>>();

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
        });
    }
}