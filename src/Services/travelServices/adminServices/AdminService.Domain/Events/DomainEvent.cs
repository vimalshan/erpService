namespace AdminService.Domain.Events;

/// <summary>
/// Base class for domain events
/// </summary>
public abstract record DomainEvent(DateTime OccurredAt)
{
    /// <summary>
    /// Unique event identifier
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Time when event occurred
    /// </summary>
    public DateTime OccurredAt { get; init; } = OccurredAt;
}
