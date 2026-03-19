using Microsoft.Extensions.Logging;
using MobileAppManagement.Application.Interfaces;

namespace MobileAppManagement.Infrastructure.Messaging;

/// <summary>
/// Stub message publisher used when RabbitMQ is unavailable.
/// Logs messages instead of publishing them.
/// </summary>
public class StubMessagePublisher(Microsoft.Extensions.Logging.ILogger<StubMessagePublisher> logger) : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        logger.LogWarning(
            "RabbitMQ unavailable. Message not published. Exchange: {Exchange}, RoutingKey: {RoutingKey}, Message: {@Message}",
            exchange, routingKey, message);
        return Task.CompletedTask;
    }
}
