using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infrastructure.Dapper;
using InventoryManagement.Infrastructure.HealthChecks;
using InventoryManagement.Infrastructure.Messaging;
using InventoryManagement.Infrastructure.Persistence;
using InventoryManagement.Infrastructure.Repositories;
using InventoryManagement.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // EF Core
        services.AddDbContext<InventoryDbContext>(opts =>
            opts.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // EF Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IUnitOfMeasureRepository, UnitOfMeasureRepository>();

        // Dapper read repos
        services.AddScoped<DapperItemReadRepository>();
        services.AddScoped<DapperProductReadRepository>();

        // RabbitMQ
        services.Configure<RabbitMqOptions>(config.GetSection(RabbitMqOptions.Section));
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddSingleton<RabbitMqInventoryConsumer>();

        // Blob Storage
        services.Configure<BlobStorageOptions>(config.GetSection(BlobStorageOptions.Section));
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        // Health Checks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database", tags: ["db", "sql"]);

        return services;
    }
}
