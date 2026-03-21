using LookupService.Application.Interfaces;
using LookupService.Domain.Interfaces;
using LookupService.Infrastructure.Dapper;
using LookupService.Infrastructure.Messaging;
using LookupService.Infrastructure.Messaging.Consumers;
using LookupService.Infrastructure.Persistence;
using LookupService.Infrastructure.Repositories;
using LookupService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace LookupService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<LookupDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(LookupDbContext).Assembly.FullName)));

        // Repositories & UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<LookupDapperQueries>();

        // RabbitMQ Publisher
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<LovSyncConsumer>();
        services.AddHostedService<ProcessSyncConsumer>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // Circuit Breaker HTTP Client
        services.AddHttpClient("LookupClient")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        // MediatR from Infrastructure assembly (for event handlers)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
