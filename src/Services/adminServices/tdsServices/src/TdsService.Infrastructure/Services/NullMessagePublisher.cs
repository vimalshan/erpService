using Microsoft.Extensions.Logging;
using TdsService.Application.Common.Interfaces;

namespace TdsService.Infrastructure.Services;

/// <summary>
/// No-op message publisher used when RabbitMQ is not available (e.g. local development).
/// </summary>
internal sealed class NullMessagePublisher : IMessagePublisher
{
    private readonly ILogger<NullMessagePublisher> _logger;

    public NullMessagePublisher(ILogger<NullMessagePublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        _logger.LogWarning("RabbitMQ unavailable — skipping publish to {Exchange}/{RoutingKey}", exchange, routingKey);
        return Task.CompletedTask;
    }
}
