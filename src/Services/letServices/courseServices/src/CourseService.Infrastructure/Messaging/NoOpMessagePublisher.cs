using Microsoft.Extensions.Logging;

namespace CourseService.Infrastructure.Messaging;

/// <summary>
/// No-op publisher used as fallback when RabbitMQ is not available (e.g. dev environment).
/// </summary>
public class NoOpMessagePublisher(ILogger logger) : IMessagePublisher
{
    public Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default)
    {
        logger.LogDebug("NoOpMessagePublisher: would publish to {RoutingKey}: {Message}", routingKey, message);
        return Task.CompletedTask;
    }
}
