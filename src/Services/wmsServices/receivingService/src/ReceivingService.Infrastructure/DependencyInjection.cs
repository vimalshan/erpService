using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using ReceivingService.Domain.Interfaces;
using ReceivingService.Infrastructure.Data;
using ReceivingService.Infrastructure.MessageBroker;
using ReceivingService.Infrastructure.MessageBroker.RabbitMQ;
using ReceivingService.Infrastructure.Repositories;
using ReceivingService.Infrastructure.Storage;

namespace ReceivingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ──────────────────────────────────────────────────────────────
        // EF Core
        // ──────────────────────────────────────────────────────────────
        services.AddDbContext<ReceivingDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ReceivingDb"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        services.AddScoped<IReceivingRepository, ReceivingRepository>();
        services.AddScoped<ReceivingDapperRepository>();

        // ──────────────────────────────────────────────────────────────
        // RabbitMQ
        // ──────────────────────────────────────────────────────────────
        services.Configure<RabbitMQSettings>(
            configuration.GetSection(RabbitMQSettings.SectionName));

        services.AddHostedService<ReceivingMessageConsumer>();

        // ──────────────────────────────────────────────────────────────
        // Azure Blob Storage
        // ──────────────────────────────────────────────────────────────
        services.AddSingleton<BlobStorageService>();

        // ──────────────────────────────────────────────────────────────
        // Polly – Circuit Breaker for downstream HTTP calls
        // ──────────────────────────────────────────────────────────────
        services.AddResiliencePipeline("default-pipeline", builder =>
        {
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio          = 0.5,
                SamplingDuration      = TimeSpan.FromSeconds(10),
                MinimumThroughput     = 8,
                BreakDuration         = TimeSpan.FromSeconds(30)
            });

            builder.AddRetry(new Polly.Retry.RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay            = TimeSpan.FromMilliseconds(200),
                BackoffType      = DelayBackoffType.Exponential
            });
        });

        return services;
    }
}
