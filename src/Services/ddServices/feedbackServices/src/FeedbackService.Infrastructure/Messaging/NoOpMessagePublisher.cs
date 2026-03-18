namespace FeedbackService.Infrastructure.Messaging;

/// <summary>
/// No-operation message publisher used when RabbitMQ is unavailable.
/// Allows the application to run without external messaging infrastructure.
/// </summary>
public class NoOpMessagePublisher : IMessagePublisher
{
    /// <summary>
    /// Publishes a message to the message broker (no-op implementation).
    /// </summary>
    /// <param name="message">The message to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Completed task</returns>
    public Task PublishAsync(object message, CancellationToken cancellationToken = default)
    {
        // No-op: silently ignore messages when broker is unavailable
        System.Diagnostics.Debug.WriteLine($"Message not published (RabbitMQ unavailable): {message?.GetType().Name}");
        return Task.CompletedTask;
    }
}
