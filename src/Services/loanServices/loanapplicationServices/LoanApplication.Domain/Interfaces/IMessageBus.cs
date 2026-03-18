namespace LoanApplication.Domain.Interfaces;

/// <summary>
/// Message bus abstraction for publishing integration events
/// </summary>
public interface IMessageBus : IAsyncDisposable
{
    /// <summary>
    /// Publish a message to an exchange with the given routing key
    /// </summary>
    Task PublishAsync<T>(T message, string routingKey, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Subscribe to messages from a queue
    /// </summary>
    Task SubscribeAsync<T>(string exchange, string queue, string routingKey, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : class;
}
