using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using SalesOrderService.Domain.Interfaces;
using SalesOrderService.Infrastructure.DomainEvents;
using SalesOrderService.Infrastructure.Messaging;
using SalesOrderService.Infrastructure.Messaging.Consumers;
using SalesOrderService.Infrastructure.Persistence;
using SalesOrderService.Infrastructure.Persistence.Repositories;
using SalesOrderService.Infrastructure.Storage;

namespace SalesOrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── EF Core ──────────────────────────────────────────────────────────
        services.AddSingleton<DomainEventDispatcherInterceptor>();

        services.AddDbContext<SalesOrderDbContext>((sp, opt) =>
        {
            opt.UseSqlServer(
                configuration.GetConnectionString("SalesOrderDb"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null));

            opt.AddInterceptors(sp.GetRequiredService<DomainEventDispatcherInterceptor>());
        });

        // ── Repositories & UoW ──────────────────────────────────────────────
        services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<SalesOrderReadRepository>();

        // ── RabbitMQ via MassTransit ─────────────────────────────────────────
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderShippedConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                var host     = configuration["RabbitMQ:Host"]     ?? "localhost";
                var vhost    = configuration["RabbitMQ:VHost"]    ?? "/";
                var user     = configuration["RabbitMQ:Username"] ?? "guest";
                var password = configuration["RabbitMQ:Password"] ?? "guest";

                cfg.Host(host, vhost, h =>
                {
                    h.Username(user);
                    h.Password(password);
                });

                cfg.ReceiveEndpoint("salesorder-shipped", e =>
                    e.ConfigureConsumer<OrderShippedConsumer>(ctx));

                cfg.ConfigureEndpoints(ctx);
            });
        });

        services.AddScoped<IEventBus, MassTransitEventBus>();

        // ── Polly Circuit Breaker (Outbound HTTP example) ───────────────────
        services.AddHttpClient("ExternalOrderClient",
            client => client.BaseAddress = new Uri(
                configuration["ExternalServices:OrderServiceUrl"] ?? "https://localhost"))
            .AddTransientHttpErrorPolicy(p =>
                p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        // ── Azure Blob Storage ───────────────────────────────────────────────
        var blobConnectionString = configuration.GetConnectionString("BlobStorage")
            ?? configuration["BlobStorage:ConnectionString"]
            ?? "UseDevelopmentStorage=true";

        services.AddSingleton(new BlobServiceClient(blobConnectionString));
        services.AddScoped<BlobStorageService>();

        return services;
    }
}
