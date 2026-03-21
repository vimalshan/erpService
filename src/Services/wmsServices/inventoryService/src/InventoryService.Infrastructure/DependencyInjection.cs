using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InventoryService.Domain.Interfaces;
using InventoryService.Infrastructure.Dapper;
using InventoryService.Infrastructure.Messaging.Consumers;
using InventoryService.Infrastructure.Persistence;
using InventoryService.Infrastructure.Services;

namespace InventoryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("InventoryDb")!;

        // EF Core
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton<IDapperInventoryQueries>(_ => new DapperInventoryQueries(connectionString));

        // Repositories / Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
        }

        // MassTransit / RabbitMQ
        services.AddMassTransit(x =>
        {
            x.AddConsumer<StockLevelChangedConsumer>();
            x.AddConsumer<LowStockAlertConsumer>();
            x.AddConsumer<InventoryTransferConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetValue<string>("RabbitMQ:Host") ?? "localhost", "/", h =>
                {
                    h.Username(configuration.GetValue<string>("RabbitMQ:Username") ?? "guest");
                    h.Password(configuration.GetValue<string>("RabbitMQ:Password") ?? "guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        // MediatR handlers from Infrastructure assembly (domain event -> message bus handlers)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
