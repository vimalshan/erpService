using Microsoft.Extensions.Logging;
using DevelopmentService.Domain.Interfaces;

namespace DevelopmentService.Infrastructure.Messaging;

/// <summary>
/// No-op publisher used when RabbitMQ is unavailable at startup.
/// Messages are logged at Warning level and discarded gracefully.
/// </summary>
internal sealed class NullMessagePublisher : IMessagePublisher
{
    private readonly ILogger<NullMessagePublisher> _logger;

    public NullMessagePublisher(ILogger<NullMessagePublisher> logger) => _logger = logger;

    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        _logger.LogWarning(
            "RabbitMQ unavailable. Skipping publish to {Exchange}/{RoutingKey}.",
            exchange, routingKey);
        return Task.CompletedTask;
    }
}
