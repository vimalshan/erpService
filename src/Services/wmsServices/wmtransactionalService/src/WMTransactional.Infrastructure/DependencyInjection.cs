using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WMTransactional.Domain.Interfaces;
using WMTransactional.Infrastructure.Dapper;
using WMTransactional.Infrastructure.Messaging.Consumers;
using WMTransactional.Infrastructure.Persistence;
using WMTransactional.Infrastructure.Services;

namespace WMTransactional.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WMTransactionalDb")!;

        // EF Core
        services.AddDbContext<WMTransactionalDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton<IDapperTransactionalQueries>(_ => new DapperTransactionalQueries(connectionString));

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
            x.AddConsumer<PurchaseOrderCreatedConsumer>();
            x.AddConsumer<PurchaseOrderStatusChangedConsumer>();
            x.AddConsumer<SalesOrderCreatedConsumer>();
            x.AddConsumer<SalesOrderStatusChangedConsumer>();
            x.AddConsumer<ShipmentShippedConsumer>();
            x.AddConsumer<ReceivingCompletedConsumer>();

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
