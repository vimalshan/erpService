namespace FeedbackService.Domain.Events;

using Common;

/// <summary>
/// Event handler for when feedback is submitted
/// </summary>
public class FeedbackSubmittedEventHandler
{
    /// <summary>
    /// Handles the FeedbackSubmittedEvent
    /// </summary>
    public async Task HandleAsync(FeedbackSubmittedEvent @event, CancellationToken cancellationToken = default)
    {
        // Example: Send notification, update related service, trigger workflow, etc.
        System.Diagnostics.Debug.WriteLine($"Feedback {(@event as dynamic).FeedbackId} was submitted at {@event.OccurredOn}");
        await Task.CompletedTask;
    }
}

/// <summary>
/// Event handler for when feedback is created
/// </summary>
public class FeedbackCreatedEventHandler
{
    /// <summary>
    /// Handles the FeedbackCreatedEvent
    /// </summary>
    public async Task HandleAsync(FeedbackCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        // Example: Log creation, initialize default data, trigger workflows, etc.
        System.Diagnostics.Debug.WriteLine($"Feedback {(@event as dynamic).FeedbackId} was created at {@event.OccurredOn}");
        await Task.CompletedTask;
    }
}
