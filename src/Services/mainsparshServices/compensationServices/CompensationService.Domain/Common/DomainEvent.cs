namespace CompensationService.Domain.Common;

/// <summary>
/// Base class for all domain events
/// </summary>
public abstract class DomainEvent
{
    public Guid AggregateId { get; }
    public DateTime OccurredAt { get; }
    public int Version { get; set; }

    protected DomainEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
        OccurredAt = DateTime.UtcNow;
    }
}
