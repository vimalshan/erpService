using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PayTransactionalService.Domain.Common;
using PayTransactionalService.Domain.Repositories;
using PayTransactionalService.Infrastructure.MessageBroker;
using PayTransactionalService.Infrastructure.Persistence;
using PayTransactionalService.Infrastructure.Repositories;

namespace PayTransactionalService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString,
        IConfiguration? configuration = null)
    {
        // Register MediatR handlers from Infrastructure
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        // Register DbContext
        services.AddDbContext<PayTransactionalDbContext>(options =>
            options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(typeof(PayTransactionalDbContext).Assembly.FullName)));

        // Register repositories
        services.AddScoped<IPayTransactionRepository, PayTransactionRepository>();
        services.AddScoped<IPayArrearRepository, PayArrearRepository>();
        services.AddScoped<IPayAdjustmentRepository, PayAdjustmentRepository>();
        services.AddScoped<IPayrollBatchRepository, PayrollBatchRepository>();

        // Register RabbitMQ
        if (configuration != null)
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQ");
            var hostName = rabbitMQConfig["HostName"] ?? "localhost";
            var userName = rabbitMQConfig["UserName"] ?? "guest";
            var password = rabbitMQConfig["Password"] ?? "guest";
            var port = int.TryParse(rabbitMQConfig["Port"], out var p) ? p : 5672;
            var virtualHost = rabbitMQConfig["VirtualHost"] ?? "/";

            services.AddSingleton<IMessageBrokerConnection>(sp =>
                new RabbitMQConnection(
                    sp.GetRequiredService<ILogger<RabbitMQConnection>>(),
                    hostName, userName, password, port, virtualHost));
            services.AddSingleton<IMessageConsumer, PayTransactionMessageConsumer>();
        }

        // Register domain event dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IReadOnlyList<DomainEvent> events, CancellationToken ct = default);
}

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    public DomainEventDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task DispatchAsync(IReadOnlyList<DomainEvent> events, CancellationToken ct = default)
    {
        foreach (var evt in events)
            await Task.CompletedTask; // Event handling implementation
    }
}
