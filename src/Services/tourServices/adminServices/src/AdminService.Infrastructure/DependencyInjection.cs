using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AdminService.Domain.Interfaces;
using AdminService.Infrastructure.Data;
using AdminService.Infrastructure.Messaging;
using AdminService.Infrastructure.Messaging.Consumers;
using AdminService.Infrastructure.Repositories;
using AdminService.Infrastructure.Resilience;
using AdminService.Infrastructure.Services;

namespace AdminService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<AdminDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton<IDapperContext>(new DapperContext(connectionString));

        // Repositories
        services.AddScoped<IAdminMasterRepository, AdminMasterRepository>();
        services.AddScoped<IAdminUserMapRepository, AdminUserMapRepository>();
        services.AddScoped<IAdminFinUserMapRepository, AdminFinUserMapRepository>();
        services.AddScoped<IAdminAccessRightsRepository, AdminAccessRightsRepository>();
        services.AddScoped<IAdminAccessRightsLogRepository, AdminAccessRightsLogRepository>();

        // RabbitMQ Publisher (graceful fallback if RabbitMQ is unavailable)
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
            try
            {
                return RabbitMqPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "RabbitMQ is unavailable. Using no-op message publisher.");
                return new NoOpMessagePublisher();
            }
        });

        // RabbitMQ Consumers (only register if RabbitMQ is enabled)
        var rabbitEnabled = configuration.GetValue("RabbitMQ:Enabled", true);
        if (rabbitEnabled)
        {
            services.AddHostedService<AdminMasterCreatedConsumer>();
            services.AddHostedService<AccessRightsGrantedConsumer>();
        }

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // Polly - HttpClient with circuit breaker
        services.AddHttpClient("ResilientClient")
            .AddPolicyHandler(PollyPolicies.GetCombinedPolicy());

        // MediatR handlers from Infrastructure (domain event handlers)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Health Checks
        services.AddHealthChecks()
            .AddSqlServer(connectionString, name: "sqlserver", tags: new[] { "db", "sql" });

        return services;
    }
}
