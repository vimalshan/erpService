using Azure.Storage.Blobs;
using EnergyService.Application.Common.Interfaces;
using EnergyService.Domain.Interfaces;
using EnergyService.Infrastructure.Dapper;
using EnergyService.Infrastructure.Messaging;
using EnergyService.Infrastructure.Messaging.Consumers;
using EnergyService.Infrastructure.Persistence;
using EnergyService.Infrastructure.Repositories;
using EnergyService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace EnergyService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("EnergyDb")!;

        // EF Core
        services.AddDbContext<EnergyDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton(new DapperContext(connectionString));
        services.AddScoped<DapperReadingRepository>();

        // Repositories
        services.AddScoped<IEcProcessRepository, EcProcessRepository>();
        services.AddScoped<IEcReadingRepository, EcReadingRepository>();
        services.AddScoped<IEcProcessAccessRepository, EcProcessAccessRepository>();
        services.AddScoped<IEcProcessMailIdRepository, EcProcessMailIdRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // RabbitMQ
        var rabbitConfig = configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>() ?? new RabbitMqConfiguration();
        services.AddSingleton<IConnectionFactory>(new ConnectionFactory
        {
            HostName = rabbitConfig.HostName,
            Port = rabbitConfig.Port,
            UserName = rabbitConfig.UserName,
            Password = rabbitConfig.Password,
            VirtualHost = rabbitConfig.VirtualHost
        });
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        services.AddHostedService<ReadingRecordedConsumer>();
        services.AddHostedService<ProcessAccessChangedConsumer>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
        }
        else
        {
            // Use local development emulator
            services.AddSingleton(new BlobServiceClient("UseDevelopmentStorage=true"));
        }
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
