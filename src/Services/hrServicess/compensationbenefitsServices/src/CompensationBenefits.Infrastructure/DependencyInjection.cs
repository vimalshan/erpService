using Azure.Storage.Blobs;
using CompensationBenefits.Domain.Interfaces;
using CompensationBenefits.Infrastructure.Dapper;
using CompensationBenefits.Infrastructure.Messaging;
using CompensationBenefits.Infrastructure.Persistence;
using CompensationBenefits.Infrastructure.Persistence.Repositories;
using CompensationBenefits.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using RabbitMQ.Client;

namespace CompensationBenefits.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ── EF Core ──────────────────────────────────────────────────────────────
        services.AddDbContext<CompensationBenefitsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null))
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

        // ── Repositories ─────────────────────────────────────────────────────────
        services.AddScoped<ISalaryRepository, SalaryRepository>();
        services.AddScoped<ISalaryStructureRepository, SalaryStructureRepository>();
        services.AddScoped<IMediclaimRepository, MediclaimRepository>();
        services.AddScoped<IMobileConnectionRepository, MobileConnectionRepository>();
        services.AddScoped<IRetiralRangeMasterRepository, RetiralRangeMasterRepository>();

        // ── Dapper ───────────────────────────────────────────────────────────────
        services.AddScoped<IDapperRepository, DapperRepository>();

        // ── Azure Blob Storage ───────────────────────────────────────────────────
        var blobConnectionString = configuration.GetConnectionString("AzureBlob") ?? "UseDevelopmentStorage=true";
        services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        // ── RabbitMQ ─────────────────────────────────────────────────────────────
        var rabbitConfig = configuration.GetSection("RabbitMQ");
        services.AddSingleton<IConnection>(_ =>
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = rabbitConfig["Host"] ?? "localhost",
                    Port = int.Parse(rabbitConfig["Port"] ?? "5672"),
                    UserName = rabbitConfig["Username"] ?? "guest",
                    Password = rabbitConfig["Password"] ?? "guest",
                    VirtualHost = rabbitConfig["VirtualHost"] ?? "/"
                };
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // RabbitMQ is optional — API continues without messaging if unavailable
                Console.WriteLine($"[RabbitMQ] Connection failed (messaging disabled): {ex.Message}");
                return null!;
            }
        });

        services.AddSingleton<Application.Contracts.IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddHostedService<SalaryEventConsumer>();
        services.AddHostedService<MediclaimEventConsumer>();
        services.AddScoped<ISalaryEventProcessor, SalaryEventProcessor>();

        // ── Polly Circuit Breaker ─────────────────────────────────────────────────
        services.AddResiliencePipeline("compensationBenefits-circuit", builder =>
        {
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(15)
            });
            builder.AddRetry(new Polly.Retry.RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(300),
                BackoffType = DelayBackoffType.Exponential
            });
        });

        // ── Health Checks ─────────────────────────────────────────────────────────
        services.AddHealthChecks()
            .AddSqlServer(
                configuration.GetConnectionString("DefaultConnection")!,
                name: "sqldb",
                tags: ["db", "sql"]);

        return services;
    }
}
