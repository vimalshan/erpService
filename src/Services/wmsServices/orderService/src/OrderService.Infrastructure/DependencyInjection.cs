using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Messaging.Consumers;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Storage;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderDb")));

        // Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<OrderDapperRepository>();

        // RabbitMQ
        var rabbitEnabled = bool.TryParse(configuration["RabbitMQ:Enabled"], out var enabled) && enabled;
        if (rabbitEnabled)
        {
            services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
            services.AddHostedService<OrderStatusUpdateConsumer>();
        }
        else
        {
            services.AddSingleton<IMessagePublisher, NoOpMessagePublisher>();
        }

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // MediatR handlers from this assembly (domain event handlers)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
