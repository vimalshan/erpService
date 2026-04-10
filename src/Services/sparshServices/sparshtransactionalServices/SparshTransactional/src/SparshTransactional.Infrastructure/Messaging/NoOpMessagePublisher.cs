using SparshTransactional.Domain.Interfaces;

namespace SparshTransactional.Infrastructure.Messaging;

public class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default) where T : class
    {
        return Task.CompletedTask;
    }
}
