namespace WebsiteContentService.Infrastructure;

using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using WebsiteContentService.Infrastructure.ExternalServices;
using WebsiteContentService.Infrastructure.Messaging;
using WebsiteContentService.Infrastructure.Messaging.Consumers;

public static class ExternalServicesExtensions
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        }

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
            Port = port
        };

        services.AddSingleton(factory);
        services.AddSingleton<IConnection>(sp =>
        {
            var connFactory = sp.GetRequiredService<ConnectionFactory>();
            try
            {
                return connFactory.CreateConnectionAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                var logger = sp.GetService<ILoggerFactory>()?.CreateLogger("RabbitMQ");
                logger?.LogWarning(ex, "RabbitMQ is not available. Messaging features will be disabled.");
                return null!;
            }
        });

        services.AddScoped<IRabbitMQService, RabbitMQService>();
        services.AddHostedService<WebsiteContentEventConsumer>();

        return services;
    }
}
