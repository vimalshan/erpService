using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StrategicStock.Application.Interfaces;
using StrategicStock.Domain.Interfaces;
using StrategicStock.Infrastructure.Dapper;
using StrategicStock.Infrastructure.Messaging;
using StrategicStock.Infrastructure.Persistence;
using StrategicStock.Infrastructure.Repositories;
using StrategicStock.Infrastructure.Services;

namespace StrategicStock.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<StrategicStockDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IStrategicStockRepository, StrategicStockRepository>();

        // Dapper
        services.AddSingleton<IDapperContext, DapperContext>();

        // Blob Storage
        var blobConnectionString = configuration["AzureBlobStorage:ConnectionString"];
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
        }
        else
        {
            services.AddSingleton<IBlobStorageService, NoOpBlobStorageService>();
        }

        // RabbitMQ — graceful fallback when broker is unavailable
        var rabbitHost = configuration["RabbitMQ:HostName"];
        if (!string.IsNullOrEmpty(rabbitHost))
        {
            services.AddSingleton<IRabbitMqPublisher>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
                try
                {
                    return RabbitMqPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "RabbitMQ is unavailable. Using no-op publisher.");
                    return new NoOpRabbitMqPublisher();
                }
            });

            // Message Consumers — only start if RabbitMQ is configured
            services.AddHostedService<StockUpdatedConsumer>();
        }
        else
        {
            services.AddSingleton<IRabbitMqPublisher, NoOpRabbitMqPublisher>();
        }

        return services;
    }
}
