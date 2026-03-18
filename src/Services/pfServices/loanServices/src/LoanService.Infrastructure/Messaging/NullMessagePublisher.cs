using LoanService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LoanService.Infrastructure.Messaging;

/// <summary>
/// No-op publisher when RabbitMQ is not available.
/// </summary>
public class NullMessagePublisher : IMessagePublisher
{
    private readonly ILogger<NullMessagePublisher> _logger;

    public NullMessagePublisher(ILogger<NullMessagePublisher> logger) => _logger = logger;

    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        _logger.LogWarning("RabbitMQ not configured. Message to {Exchange}/{RoutingKey} was not published.", exchange, routingKey);
        return Task.CompletedTask;
    }
}
