using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Messaging;

public class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
