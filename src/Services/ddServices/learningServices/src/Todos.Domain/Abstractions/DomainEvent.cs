namespace Todos.Domain.Abstractions;

/// <summary>
/// Base class for all domain events
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Gets the event creation timestamp
    /// </summary>
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the aggregate ID that raised this event
    /// </summary>
    public Guid AggregateId { get; set; }

    /// <summary>
    /// Gets the event version
    /// </summary>
    public int Version { get; set; } = 1;
}
