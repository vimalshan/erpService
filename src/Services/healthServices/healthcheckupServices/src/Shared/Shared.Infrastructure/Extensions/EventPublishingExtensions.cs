using RabbitMQ.Client;
using Shared.Infrastructure.EventPublishing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using MediatR;

namespace Shared.Infrastructure.Extensions;

/// <summary>
/// Extension methods for event publishing and RabbitMQ setup
/// </summary>
public static class EventPublishingExtensions
{
    /// <summary>
    /// Add RabbitMQ event publishing to the service collection
    /// </summary>
    public static IServiceCollection AddRabbitMqEventPublishing(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            // Get RabbitMQ configuration
            var rabbitMqHost = configuration.GetConnectionString("RabbitMQ") ?? "amqp://guest:guest@localhost:5672";

            // Create RabbitMQ connection
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMqHost),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            };

            var connection = factory.CreateConnection();
            services.AddSingleton(connection);

            // Register event publisher
            services.AddScoped<RabbitMqEventPublisher>();

            // Register MediatR notification handlers for events
            // These are discovered automatically by MediatR, but we ensure they're registered
            var handlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && !p.IsAbstract)
                .Where(p => p.GetInterfaces().Any(i => 
                    i.IsGenericType && 
                    i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                .ToList();

            foreach (var handler in handlers)
            {
                var notificationHandlerInterface = handler.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && 
                        i.GetGenericTypeDefinition() == typeof(INotificationHandler<>));

                if (notificationHandlerInterface != null)
                {
                    services.AddScoped(notificationHandlerInterface, handler);
                }
            }

            return services;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to configure RabbitMQ event publishing", ex);
        }
    }

    /// <summary>
    /// Use RabbitMQ event publishing (called in Program.cs setup)
    /// </summary>
    public static WebApplicationBuilder UseRabbitMqEventPublishing(this WebApplicationBuilder builder)
    {
        builder.Services.AddRabbitMqEventPublishing(builder.Configuration);
        return builder;
    }
}
