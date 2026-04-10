using TaskTransactional.Application.Interfaces;
using TaskTransactional.Domain.Interfaces;
using TaskTransactional.Infrastructure.Dapper;
using TaskTransactional.Infrastructure.Messaging;
using TaskTransactional.Infrastructure.Messaging.Consumers;
using TaskTransactional.Infrastructure.Persistence;
using TaskTransactional.Infrastructure.Repositories;
using TaskTransactional.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace TaskTransactional.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ComplaintDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ComplaintDbContext).Assembly.FullName)));

        // Repositories & UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddScoped<ComplaintDapperQueries>();

        // RabbitMQ Publisher
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<ComplaintSyncConsumer>();
        services.AddHostedService<TicketSyncConsumer>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // Circuit Breaker HTTP Client
        services.AddHttpClient("ComplaintClient")
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
