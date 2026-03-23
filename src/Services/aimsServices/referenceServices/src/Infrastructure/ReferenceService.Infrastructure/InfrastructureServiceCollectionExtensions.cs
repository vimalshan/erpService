using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReferenceService.Infrastructure.Persistence;
using ReferenceService.Infrastructure.Repositories;
using ReferenceService.Infrastructure.RabbitMQ;
using ReferenceService.Domain.Interfaces;

namespace ReferenceService.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString,
        IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ReferenceDbContext>((provider, options) =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });
        
        // Register repositories
        services.AddScoped<ILovTypeRepository, LovTypeRepository>();
        services.AddScoped<ILovValueRepository, LovValueRepository>();
        services.AddScoped<IPermissionRuleRepository, PermissionRuleRepository>();
        services.AddScoped<ILeaveFlagRepository, LeaveFlagRepository>();
        
        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register RabbitMQ (async API — RabbitMQ.Client 7.x compatible)
        var rabbitCfg = configuration.GetSection("RabbitMQ").Get<RabbitMQConfiguration>()
                        ?? new RabbitMQConfiguration();
        services.AddSingleton(rabbitCfg);
        services.AddSingleton<RabbitMQConnectionFactory>();
        services.AddSingleton<RabbitMQPublisher>();
        services.AddHostedService<RabbitMQConsumerHostedService>();
        
        return services;
    }
}
