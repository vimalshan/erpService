using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using CompensationService.Infrastructure.ExternalServices;
using CompensationService.Infrastructure.Messaging;
using Azure.Storage.Blobs;

namespace CompensationService.Infrastructure;

/// <summary>
/// Extension methods for registering external services
/// </summary>
public static class ExternalServicesExtensions
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Azure Blob Storage
        services.AddSingleton(x => new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ
        var rabbitMQSettings = configuration.GetSection("RabbitMQ");
        var hostName = rabbitMQSettings["HostName"] ?? "localhost";
        var userName = rabbitMQSettings["UserName"] ?? "guest";
        var password = rabbitMQSettings["Password"] ?? "guest";
        var portStr = rabbitMQSettings["Port"] ?? "5672";
        var port = int.TryParse(portStr, out var p) ? p : 5672;
        
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = port,
            DispatchConsumersAsync = true
        };

        services.AddSingleton(factory);
        services.AddSingleton<IConnection>(sp =>
        {
            var connFactory = sp.GetRequiredService<ConnectionFactory>();
            try
            {
                return connFactory.CreateConnection();
            }
            catch (Exception ex)
            {
                var logger = sp.GetService<ILoggerFactory>()?.CreateLogger("RabbitMQ");
                logger?.LogWarning(ex, "RabbitMQ is not available. Messaging features will be disabled.");
                return null!;
            }
        });
        services.AddScoped<IRabbitMQService, RabbitMQService>();

        return services;
    }
}
