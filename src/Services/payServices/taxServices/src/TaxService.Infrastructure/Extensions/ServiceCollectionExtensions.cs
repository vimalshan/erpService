using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using TaxService.Domain.Repositories;
using TaxService.Infrastructure.Data;
using TaxService.Infrastructure.MessageBroker;
using TaxService.Infrastructure.Repositories;

namespace TaxService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString,
        IConfiguration? configuration = null)
    {
        // Register DbContext
        services.AddDbContext<TaxServiceDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly(typeof(TaxServiceDbContext).Assembly.FullName)));

        // Register repositories
        services.AddScoped<ITaxMarginalDetailRepository, TaxMarginalDetailRepository>();
        services.AddScoped<IConditionalMasterRepository, ConditionalMasterRepository>();

        // Register RabbitMQ
        if (configuration != null)
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQ");
            var hostName = rabbitMQConfig["HostName"] ?? "localhost";
            var userName = rabbitMQConfig["UserName"] ?? "guest";
            var password = rabbitMQConfig["Password"] ?? "guest";

            services.AddSingleton<IMessageBrokerConnection>(sp =>
                new RabbitMQConnection(
                    sp.GetRequiredService<ILogger<RabbitMQConnection>>(),
                    hostName,
                    userName,
                    password));
            services.AddSingleton<IMessageConsumer, TaxEventMessageConsumer>();
        }

        // Register Polly policies
        RegisterPollyPolicies(services);

        // Register domain event handler
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }

    private static void RegisterPollyPolicies(IServiceCollection services)
    {
        // This is a placeholder for Polly policy configuration
        // Polly policies would be configured here for circuit breaker, retry, timeout, etc.
        // For now, we're keeping it minimal to avoid dependency version issues
    }
}

/// <summary>
/// Domain event dispatcher for handling domain events
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IReadOnlyList<TaxService.Domain.Common.DomainEvent> events, 
        CancellationToken cancellationToken = default);
}

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(
        IReadOnlyList<TaxService.Domain.Common.DomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        foreach (var evt in events)
        {
            // TODO: Implement event handling for each domain event type
            await Task.CompletedTask;
        }
    }
}
