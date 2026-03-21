using AuditLogService.Domain.Repositories;
using AuditLogService.Infrastructure.BlobStorage;
using AuditLogService.Infrastructure.Messaging;
using AuditLogService.Infrastructure.Persistence;
using AuditLogService.Infrastructure.Persistence.Dapper;
using AuditLogService.Infrastructure.Persistence.Repositories;
using AuditLogService.Infrastructure.Resilience;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditLogService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<AuditLogDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton(new AuditLogDapperRepository(connectionString));

        // Repositories
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        // RabbitMQ
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
        services.AddSingleton<RabbitMqPublisher>();
        services.AddHostedService<AuditLogConsumer>();

        // Blob Storage
        services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
        services.AddSingleton<BlobStorageService>();

        // Polly - HttpClient with circuit breaker
        services.AddHttpClient("ExternalApi")
            .AddPolicyHandler(CircuitBreakerPolicies.GetCombinedPolicy());

        return services;
    }
}
