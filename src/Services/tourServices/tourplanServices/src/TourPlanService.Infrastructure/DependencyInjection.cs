using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TourPlanService.Application.Interfaces;
using TourPlanService.Domain.Interfaces;
using TourPlanService.Infrastructure.BlobStorage;
using TourPlanService.Infrastructure.Data;
using TourPlanService.Infrastructure.DapperQueries;
using TourPlanService.Infrastructure.HealthChecks;
using TourPlanService.Infrastructure.Messaging.RabbitMq;
using TourPlanService.Infrastructure.Repositories;

namespace TourPlanService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<TourPlanDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TourPlanDb"),
                sql => sql.MigrationsAssembly(typeof(TourPlanDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ITourPlanRepository, TourPlanRepository>();
        services.AddScoped<IForexRepository, ForexRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<TourPlanDapperRepository>();

        // RabbitMQ
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.Section));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<TourPlanCreatedConsumer>();
        services.AddHostedService<TourPlanApprovedConsumer>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrWhiteSpace(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // Health Checks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database", tags: ["db", "sql"]);

        // Polly Circuit Breaker for HttpClient
        services.AddHttpClient("ExternalApi", client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalApiBaseUrl"] ?? "https://localhost");
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddPolicyHandler(GetCircuitBreakerPolicy())
        .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
