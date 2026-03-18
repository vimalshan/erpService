using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ReferenceService.Infrastructure.Persistence;
using ReferenceService.Infrastructure.Repositories;
// using ReferenceService.Infrastructure.RabbitMQ;
// using ReferenceService.Infrastructure.DomainEventPublisher;
using ReferenceService.Domain.Interfaces;

namespace ReferenceService.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString)
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
        services.AddScoped<LovTypeRepository>();
        services.AddScoped<LovValueRepository>();
        services.AddScoped<PermissionRuleRepository>();
        services.AddScoped<LeaveFlagRepository>();
        
        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Register RabbitMQ
        // NOTE: RabbitMQ configuration is currently disabled due to API compatibility issues with RabbitMQ.Client 7.1.0
        // if (rabbitMqConfig != null)
        // {
        //     services.AddSingleton(rabbitMqConfig);
        //     services.AddSingleton<RabbitMQConnectionFactory>();
        //     services.AddScoped<IDomainEventPublisher, RabbitMQDomainEventPublisher>(sp =>
        //         new RabbitMQDomainEventPublisher(
        //             sp.GetRequiredService<RabbitMQConnectionFactory>().GetConnection()
        //         )
        //     );
        // }
        
        return services;
    }
}
