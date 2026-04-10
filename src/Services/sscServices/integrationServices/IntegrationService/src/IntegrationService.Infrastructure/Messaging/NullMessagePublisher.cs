using IntegrationService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace IntegrationService.Infrastructure.Messaging;

public class NullMessagePublisher(ILogger<NullMessagePublisher> logger) : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default) where T : class
    {
        logger.LogWarning("NullMessagePublisher: Skipping publish to {Exchange}/{RoutingKey} (RabbitMQ unavailable)", exchange, routingKey);
        return Task.CompletedTask;
    }
}
