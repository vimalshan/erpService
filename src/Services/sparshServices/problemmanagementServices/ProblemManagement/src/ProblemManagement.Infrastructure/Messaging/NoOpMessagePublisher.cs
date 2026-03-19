using ProblemManagement.Domain.Interfaces;

namespace ProblemManagement.Infrastructure.Messaging;

/// <summary>
/// No-op implementation of IMessagePublisher for when RabbitMQ is disabled.
/// </summary>
public class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default) where T : class
    {
        // Do nothing - just return a completed task
        return Task.CompletedTask;
    }
}
