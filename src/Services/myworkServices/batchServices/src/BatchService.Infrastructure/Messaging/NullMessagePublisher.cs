using BatchService.Domain.Interfaces;

namespace BatchService.Infrastructure.Messaging;

/// <summary>No-op publisher used when RabbitMQ is unavailable.</summary>
public sealed class NullMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class
        => Task.CompletedTask;
}
