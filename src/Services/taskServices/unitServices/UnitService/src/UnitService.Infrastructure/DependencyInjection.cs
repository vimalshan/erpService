using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UnitService.Application.Interfaces;
using UnitService.Domain.Interfaces;
using UnitService.Infrastructure.Dapper;
using UnitService.Infrastructure.Data;
using UnitService.Infrastructure.Messaging;
using UnitService.Infrastructure.Messaging.Consumers;
using UnitService.Infrastructure.Repositories;
using UnitService.Infrastructure.Services;

namespace UnitService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<UnitDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton(new DapperContext(connectionString));

        // Repositories
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IEquipmentStatusRepository, EquipmentStatusRepository>();
        services.AddScoped<IAccessRepository, AccessRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IMailIdRepository, MailIdRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ Publisher
        var rabbitHost = configuration.GetValue<string>("RabbitMQ:HostName") ?? "localhost";
        var rabbitUser = configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest";
        var rabbitPass = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";

        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
            try
            {
                return RabbitMqPublisher.CreateAsync(rabbitHost, rabbitUser, rabbitPass, logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "RabbitMQ publisher could not connect. Publishing will be unavailable.");
                return new NoOpMessagePublisher();
            }
        });

        // RabbitMQ Consumers
        services.AddSingleton(sp => new EquipmentRegisteredConsumer(
            sp.GetRequiredService<ILogger<EquipmentRegisteredConsumer>>(), rabbitHost, rabbitUser, rabbitPass));
        services.AddHostedService(sp => sp.GetRequiredService<EquipmentRegisteredConsumer>());

        services.AddSingleton(sp => new EquipmentStatusChangedConsumer(
            sp.GetRequiredService<ILogger<EquipmentStatusChangedConsumer>>(), rabbitHost, rabbitUser, rabbitPass));
        services.AddHostedService(sp => sp.GetRequiredService<EquipmentStatusChangedConsumer>());

        return services;
    }
}
