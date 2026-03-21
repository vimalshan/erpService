using AgencyService.Infrastructure.BlobStorage;
using AgencyService.Infrastructure.Messaging;
using AgencyService.Infrastructure.Resilience;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace AgencyService.Infrastructure;

public static class AdvancedServiceCollectionExtensions
{
    public static IServiceCollection AddAdvancedFeatures(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // RabbitMQ Messaging
        services.AddRabbitMQMessaging(configuration);
        
        // Polly Circuit Breaker & Resilience
        services.AddPollyPolicies();
        
        // Azure Blob Storage
        services.AddBlobStorage(configuration);
        
        return services;
    }
}
