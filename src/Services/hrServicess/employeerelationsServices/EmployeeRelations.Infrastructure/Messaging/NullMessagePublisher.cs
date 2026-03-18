namespace EmployeeRelations.Infrastructure.Messaging;

/// <summary>
/// No-op publisher used when RabbitMQ is unavailable at startup.
/// Messages are silently discarded, allowing the API to remain functional
/// for non-messaging operations.
/// </summary>
internal sealed class NullMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
        => Task.CompletedTask;
}
