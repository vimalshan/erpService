using EximManagement.Application.Interfaces;
using EximManagement.Infrastructure.Data;
using EximManagement.Infrastructure.Messaging;
using EximManagement.Infrastructure.Repositories;
using EximManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace EximManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<EximDbContext>(opts =>
            opts.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        // Repositories
        services.AddScoped<IEximDataFileRepository, EximDataFileRepository>();
        services.AddScoped<IEximProductRepository, EximProductRepository>();
        services.AddScoped<IEximProductGroupRepository, EximProductGroupRepository>();
        services.AddScoped<IEximDataExportRepository, EximDataExportRepository>();
        services.AddScoped<IEximDataImportRepository, EximDataImportRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<EximDapperService>();
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // RabbitMQ Consumer (Background Service)
        services.AddHostedService<EximMessageConsumer>();

        // Polly Circuit Breaker - example for HTTP clients
        services.AddHttpClient("EximExternalApi", client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalApi:BaseUrl"] ?? "https://localhost");
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddPolicyHandler(GetCircuitBreakerPolicy())
        .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
