using DeductionService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DeductionService.Infrastructure.Messaging;

/// <summary>
/// Fallback publisher used when RabbitMQ is not available.
/// Logs a warning that the message was dropped rather than crashing the host.
/// </summary>
public class NullMessagePublisher(ILogger<NullMessagePublisher> logger) : IMessagePublisher
{
    public Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default) where T : class
    {
        logger.LogWarning("[RabbitMQ] Broker unavailable — message dropped. RoutingKey={RoutingKey}, Type={Type}",
            routingKey, typeof(T).Name);
        return Task.CompletedTask;
    }
}
