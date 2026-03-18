namespace FeedbackService.Domain.Common;

/// <summary>
/// Abstract base class for domain events
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Gets the date/time when the event occurred
    /// </summary>
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
