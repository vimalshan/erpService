using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BatchService.Domain.Interfaces;
using BatchService.Infrastructure.Dapper;
using BatchService.Infrastructure.Messaging;
using BatchService.Infrastructure.Messaging.Consumers;
using BatchService.Infrastructure.Persistence;
using BatchService.Infrastructure.Repositories;
using BatchService.Infrastructure.Storage;

namespace BatchService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // ── EF Core ───────────────────────────────────────────────────────
        services.AddDbContext<BatchDbContext>(opts =>
            opts.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3)));

        // ── Repositories ──────────────────────────────────────────────────
        services.AddScoped<IBatchRepository, BatchRepository>();

        // ── Dapper (read-side) ────────────────────────────────────────────
        services.AddSingleton(sp =>
            new DapperBatchReadRepository(config.GetConnectionString("DefaultConnection")!));

        // ── Azure Blob Storage ────────────────────────────────────────────
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // ── RabbitMQ Publisher (singleton, async init) ────────────────────
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var innerConfig = sp.GetRequiredService<IConfiguration>();
            var logger      = sp.GetRequiredService<ILogger<RabbitMQPublisher>>();
            try
            {
                return RabbitMQPublisher.CreateAsync(innerConfig, logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "[RabbitMQ] Could not connect. Publishing is disabled.");
                return new NullMessagePublisher();
            }
        });

        // ── RabbitMQ Consumer (background) ────────────────────────────────
        services.AddHostedService<BatchMessageConsumer>();

        return services;
    }
}
