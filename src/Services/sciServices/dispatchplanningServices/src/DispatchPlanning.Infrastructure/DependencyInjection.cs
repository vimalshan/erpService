using DispatchPlanning.Domain.Interfaces;
using DispatchPlanning.Infrastructure.DomainEvents;
using DispatchPlanning.Infrastructure.Messaging;
using DispatchPlanning.Infrastructure.Persistence;
using DispatchPlanning.Infrastructure.Repositories;
using DispatchPlanning.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;

namespace DispatchPlanning.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<DispatchPlanningDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SCIDB"),
                sql => sql.MigrationsAssembly(typeof(DispatchPlanningDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IDispatchPlanRepository, DispatchPlanRepository>();
        services.AddScoped<IDispatchPlanMainGroupRepository, DispatchPlanMainGroupRepository>();
        services.AddScoped<IDispatchPlanSubGroupRepository, DispatchPlanSubGroupRepository>();
        services.AddScoped<IDispatchPlanBreakupItemRepository, DispatchPlanBreakupItemRepository>();

        // Domain Event Dispatcher
        services.AddScoped<DomainEventDispatcher>();

        // Messaging
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<DispatchPlanCreatedConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // Polly Circuit Breaker on HttpClients
        services.AddHttpClient("ResilienceClient")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
