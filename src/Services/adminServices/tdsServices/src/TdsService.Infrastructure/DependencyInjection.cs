using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using TdsService.Application.Common.Interfaces;
using TdsService.Domain.Repositories;
using TdsService.Infrastructure.Dapper;
using TdsService.Infrastructure.Messaging;
using TdsService.Infrastructure.Persistence;
using TdsService.Infrastructure.Persistence.Repositories;
using TdsService.Infrastructure.Services;

namespace TdsService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── EF Core ──────────────────────────────────────────────────
        var connectionString = configuration.GetConnectionString("TdsDb")
            ?? throw new InvalidOperationException("Connection string 'TdsDb' is missing.");

        services.AddDbContext<TdsDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sql => sql.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<TdsDbContext>());

        // ── Repositories (EF) ────────────────────────────────────────
        services.AddScoped<ITdsVendorRepository, TdsVendorRepository>();
        services.AddScoped<ITdsFileRepository, TdsFileRepository>();

        // ── Repositories (Dapper — read-model queries) ───────────────
        services.AddScoped<ITdsVendorDapperRepository>(_ =>
            new TdsVendorDapperRepository(connectionString));
        services.AddScoped<ITdsFileDapperRepository>(_ =>
            new TdsFileDapperRepository(connectionString));

        // ── Azure Blob Storage ────────────────────────────────────────
        var blobConnectionString = configuration["AzureStorage:ConnectionString"];
        if (!string.IsNullOrWhiteSpace(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }
        else
        {
            services.AddScoped<IBlobStorageService, NullBlobStorageService>();
        }

        // ── RabbitMQ publisher (with Polly circuit-breaker) ──────────
        var rabbitHost = configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitUser = configuration["RabbitMQ:Username"] ?? "guest";
        var rabbitPass = configuration["RabbitMQ:Password"] ?? "guest";

        // Try to connect to RabbitMQ; fall back to no-op publisher if unavailable
        bool rabbitAvailable = false;
        try
        {
            var testFactory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = rabbitHost,
                UserName = rabbitUser,
                Password = rabbitPass
            };
            using var testConn = testFactory.CreateConnectionAsync().GetAwaiter().GetResult();
            rabbitAvailable = true;
        }
        catch
        {
            // RabbitMQ not reachable
        }

        if (rabbitAvailable)
        {
            services.AddScoped<IMessagePublisher>(_ =>
            {
                var pipeline = new ResiliencePipelineBuilder()
                    .AddCircuitBreaker(new CircuitBreakerStrategyOptions
                    {
                        FailureRatio = 0.5,
                        SamplingDuration = TimeSpan.FromSeconds(30),
                        MinimumThroughput = 5,
                        BreakDuration = TimeSpan.FromSeconds(30)
                    })
                    .AddRetry(new Polly.Retry.RetryStrategyOptions
                    {
                        MaxRetryAttempts = 3,
                        Delay = TimeSpan.FromSeconds(2)
                    })
                    .Build();

                return new PollyWrappedMessagePublisher(
                    RabbitMqMessagePublisher.CreateAsync(rabbitHost, rabbitUser, rabbitPass)
                        .GetAwaiter().GetResult(),
                    pipeline);
            });

            // ── RabbitMQ consumer (background service) ───────────────────
            services.AddHostedService(sp => new TdsEmailConfirmationConsumer(
                sp.GetRequiredService<IServiceScopeFactory>(),
                sp.GetRequiredService<ILogger<TdsEmailConfirmationConsumer>>(),
                rabbitHost, rabbitUser, rabbitPass));
        }
        else
        {
            services.AddScoped<IMessagePublisher>(sp =>
                new NullMessagePublisher(sp.GetRequiredService<ILogger<NullMessagePublisher>>()));
        }

        return services;
    }
}
