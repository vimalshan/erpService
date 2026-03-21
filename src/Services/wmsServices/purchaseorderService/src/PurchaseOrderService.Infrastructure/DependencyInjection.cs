using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOrderService.Application.Interfaces;
using PurchaseOrderService.Domain.Interfaces;
using PurchaseOrderService.Infrastructure.BlobStorage;
using PurchaseOrderService.Infrastructure.Messaging;
using PurchaseOrderService.Infrastructure.Messaging.Consumers;
using PurchaseOrderService.Infrastructure.Persistence;
using PurchaseOrderService.Infrastructure.Repositories;
using Polly;
using Polly.Extensions.Http;

namespace PurchaseOrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<PurchaseOrderDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(PurchaseOrderDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<IPurchaseOrderReadRepository, PurchaseOrderReadRepository>();

        // Messaging
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // Message Consumers
        services.AddHostedService<SupplierUpdatedConsumer>();
        services.AddHostedService<InventoryReceivedConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // MediatR handlers from this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Polly Circuit Breaker for HttpClient
        services.AddHttpClient("ExternalApi")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

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
