using AuditService.Application.Interfaces;
using AuditService.Domain.Interfaces;
using AuditService.Infrastructure.Authentication;
using AuditService.Infrastructure.BlobStorage;
using AuditService.Infrastructure.Dapper;
using AuditService.Infrastructure.Data;
using AuditService.Infrastructure.HealthChecks;
using AuditService.Infrastructure.Messaging;
using AuditService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Entity Framework
        services.AddDbContext<AuditDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(30);
                }));

        // Repositories
        services.AddScoped<IAuditRepository, AuditRepository>();
        services.AddScoped<IObservationRepository, ObservationRepository>();
        services.AddScoped<IGoodPracticeRepository, GoodPracticeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddSingleton<DapperContext>();
        services.AddScoped<AuditDapperRepository>();

        // Authentication
        services.AddScoped<IJwtService, JwtService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

        // Health Checks
        var rabbitHost = configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitPort = int.Parse(configuration["RabbitMQ:Port"] ?? "5672");

        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "db", "sql" })
            .AddCheck("rabbitmq", new RabbitMQHealthCheck(rabbitHost, rabbitPort), tags: new[] { "messaging" });

        return services;
    }
}
