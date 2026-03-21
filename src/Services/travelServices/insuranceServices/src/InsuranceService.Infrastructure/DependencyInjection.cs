using Azure.Storage.Blobs;
using InsuranceService.Domain.Repositories;
using InsuranceService.Infrastructure.BlobStorage;
using InsuranceService.Infrastructure.Dapper;
using InsuranceService.Infrastructure.Messaging;
using InsuranceService.Infrastructure.Messaging.Consumers;
using InsuranceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace InsuranceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<InsuranceDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<ITravelInsuranceRepository, TravelInsuranceRepository>();

        // Dapper
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        services.AddSingleton<IDapperInsuranceQuery>(new DapperInsuranceQuery(connectionString));

        // RabbitMQ (optional – gracefully degrades if broker is unavailable)
        var rabbitConfig = configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory
        {
            HostName = rabbitConfig["HostName"] ?? "localhost",
            UserName = rabbitConfig["UserName"] ?? "guest",
            Password = rabbitConfig["Password"] ?? "guest",
            Port = int.TryParse(rabbitConfig["Port"], out var port) ? port : 5672
        };

        services.AddSingleton<IConnectionFactory>(factory);
        // RabbitMQ Consumers (only register if RabbitMQ is enabled)
        var rabbitEnabled = configuration.GetValue<bool?>("RabbitMQ:Enabled") ?? true;
        if (rabbitEnabled)
        {
            try
            {
                var testConnection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                services.AddSingleton(testConnection);
                services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
                services.AddHostedService<InsuranceRegistrationConsumer>();
                services.AddHostedService<InsuranceStatusUpdateConsumer>();
            }
            catch
            {
                // RabbitMQ unavailable – register no-op publisher
                services.AddSingleton<IMessagePublisher, NoOpMessagePublisher>();
            }
        }
        else
        {
            services.AddSingleton<IMessagePublisher, NoOpMessagePublisher>();
        }

        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
        }

        // MediatR event handlers from this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
