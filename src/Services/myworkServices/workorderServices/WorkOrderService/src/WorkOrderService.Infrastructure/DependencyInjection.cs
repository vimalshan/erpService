using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using WorkOrderService.Application.Interfaces;
using WorkOrderService.Domain.Interfaces;
using WorkOrderService.Infrastructure.Dapper;
using WorkOrderService.Infrastructure.Messaging;
using WorkOrderService.Infrastructure.Messaging.Consumers;
using WorkOrderService.Infrastructure.Persistence;
using WorkOrderService.Infrastructure.Repositories;
using WorkOrderService.Infrastructure.Services;

namespace WorkOrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<WorkOrderDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton<IDapperContext>(new DapperContext(connectionString));

        // Repositories
        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        services.AddScoped<IWorkTaskRepository, WorkTaskRepository>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ
        var rabbitHost = configuration.GetValue<string>("RabbitMQ:HostName") ?? "localhost";
        var rabbitUser = configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest";
        var rabbitPass = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";

        services.AddSingleton(new ConnectionFactory
        {
            HostName = rabbitHost,
            UserName = rabbitUser,
            Password = rabbitPass
        });

        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var factory = sp.GetRequiredService<ConnectionFactory>();
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>();
            return new RabbitMqPublisher(factory, logger);
        });

        // Message Consumers
        services.AddHostedService<TaskCompletionConsumer>();
        services.AddHostedService<WorkOrderCreationConsumer>();

        return services;
    }
}
