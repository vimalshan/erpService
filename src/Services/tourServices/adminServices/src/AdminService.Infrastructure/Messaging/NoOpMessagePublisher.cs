using Microsoft.Extensions.Logging;

namespace AdminService.Infrastructure.Messaging;

public class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string exchangeName, string routingKey, T message, CancellationToken ct = default)
    {
        // No-op: RabbitMQ is not available
        return Task.CompletedTask;
    }
}
