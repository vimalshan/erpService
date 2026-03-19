using FilingAndArchiveService.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FilingAndArchiveService.Infrastructure.Services;

/// <summary>
/// Fallback publisher used when RabbitMQ is unavailable. Logs messages instead of publishing.
/// </summary>
public class NoOpMessagePublisher : IMessagePublisher
{
    private readonly ILogger<NoOpMessagePublisher> _logger;

    public NoOpMessagePublisher(ILogger<NoOpMessagePublisher> logger) => _logger = logger;

    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogWarning("RabbitMQ unavailable. Message to {Exchange}/{RoutingKey} was not published.", exchange, routingKey);
        return Task.CompletedTask;
    }
}
