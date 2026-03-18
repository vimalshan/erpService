using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ItemMasterService.Domain.Interfaces;
using ItemMasterService.Infrastructure.DomainEvents;
using ItemMasterService.Infrastructure.Messaging.Consumers;
using ItemMasterService.Infrastructure.Messaging.RabbitMQ;
using ItemMasterService.Infrastructure.Persistence.Dapper;
using ItemMasterService.Infrastructure.Persistence.EF;
using ItemMasterService.Infrastructure.Persistence.Repositories;
using ItemMasterService.Infrastructure.Resilience;
using ItemMasterService.Infrastructure.Storage;

namespace ItemMasterService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        // EF Core
        services.AddDbContext<ItemMasterDbContext>(options =>
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ItemMasterDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ICanteenItemRepository, CanteenItemRepository>();
        services.AddScoped<ICanteenItemPriceRepository, CanteenItemPriceRepository>();
        services.AddScoped<ICanteenGradeItemPriceRepository, CanteenGradeItemPriceRepository>();

        // Dapper
        services.AddSingleton(sp => new CanteenItemDapperRepository(
            config.GetConnectionString("DefaultConnection")!));

        // Blob Storage
        services.Configure<BlobStorageSettings>(config.GetSection("BlobStorage"));
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // RabbitMQ
        services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

        // Message Consumers (Background Services)
        services.AddHostedService<CanteenItemCreatedConsumer>();
        services.AddHostedService<CanteenItemPriceUpdatedConsumer>();

        // Resilience
        services.AddSingleton<CircuitBreakerPolicy>();

        // Domain Event Dispatcher
        services.AddScoped<DomainEventDispatcher>();

        return services;
    }
}
