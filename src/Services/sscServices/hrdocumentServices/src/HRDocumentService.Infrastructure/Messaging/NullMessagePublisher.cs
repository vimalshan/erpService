using HRDocumentService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HRDocumentService.Infrastructure.Messaging;

/// <summary>
/// Fallback publisher that logs messages when RabbitMQ is unavailable.
/// </summary>
public sealed class NullMessagePublisher(ILogger<NullMessagePublisher> logger) : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        logger.LogWarning("RabbitMQ unavailable. Message to {Exchange}/{RoutingKey} was not sent: {@Message}",
            exchange, routingKey, message);
        return Task.CompletedTask;
    }
}
