using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using CardManagement.Application.Common.Interfaces;
using CardManagement.Domain.Interfaces;
using CardManagement.Infrastructure.Messaging.Consumers;
using CardManagement.Infrastructure.Persistence;
using CardManagement.Infrastructure.Persistence.Dapper;
using CardManagement.Infrastructure.Persistence.Repositories;
using CardManagement.Infrastructure.Services;

namespace CardManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionString 'DefaultConnection' is not configured.");

        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                sql.CommandTimeout(120);
            }));

        services.AddScoped<IApplicationDbContext>(p => p.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IGuestCardMasterRepository, GuestCardMasterRepository>();
        services.AddScoped<ICanteenCardMapRepository, CanteenCardMapRepository>();
        services.AddScoped<ICardSettlementRepository, CardSettlementRepository>();
        services.AddSingleton<ICardDapperRepository>(_ => new CardDapperRepository(connectionString));

        // Azure Blob Storage
        var blobConn = config.GetConnectionString("BlobStorage") ?? "UseDevelopmentStorage=true";
        services.AddSingleton(_ => new BlobServiceClient(blobConn));
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // MassTransit — use RabbitMQ when enabled, otherwise InMemory (useful for dev without a broker)
        var rabbitEnabled = !string.Equals(config["RabbitMQ:Enabled"], "false", StringComparison.OrdinalIgnoreCase);
        services.AddMassTransit(x =>
        {
            x.AddConsumer<GuestCardCreatedConsumer>();
            x.AddConsumer<GuestCardClosedConsumer>();
            x.AddConsumer<CardSettledConsumer>();

            if (rabbitEnabled)
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    var rabbit = config.GetSection("RabbitMQ");
                    cfg.Host(rabbit["Host"] ?? "localhost", rabbit["VirtualHost"] ?? "/", h =>
                    {
                        h.Username(rabbit["Username"] ?? "guest");
                        h.Password(rabbit["Password"] ?? "guest");
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
            }
            else
            {
                x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));
            }
        });

        services.AddSingleton(sp => new RabbitMqAvailability(rabbitEnabled));

        services.AddScoped<IMessagePublisher, MassTransitMessagePublisher>();

        // Polly Circuit Breaker
        services.AddSingleton<ResiliencePipeline>(sp =>
            new ResiliencePipelineBuilder()
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 10,
                    BreakDuration = TimeSpan.FromSeconds(15)
                })
                .AddRetry(new Polly.Retry.RetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromMilliseconds(200),
                    BackoffType = DelayBackoffType.Exponential
                })
                .Build());

        return services;
    }
}

/// <summary>Indicates whether RabbitMQ transport is active (used in health check registration).</summary>
public record RabbitMqAvailability(bool IsEnabled);
