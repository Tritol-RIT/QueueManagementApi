using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using QueueManagementApi.Application.Services;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;
using QueueManagementApi.Infrastructure.Data;
using QueueManagementApi.Infrastructure.Repositories;
using QueueManagementApi.Infrastructure.UnitOfWork;

namespace QueueManagementApi.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddDbContextCustom(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbContext, QueueManagementDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres"), npgsqloptions => { }));
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IExhibitService, ExhibitService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Exhibit>, GenericRepository<Exhibit>>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}