namespace ReportingService.Domain.Events;

/// <summary>
/// Base domain event class
/// </summary>
public abstract class DomainEvent
{
    public long AggregateId { get; }
    public DateTime OccurredAt { get; }
    public int Version { get; protected set; }

    protected DomainEvent(long aggregateId, DateTime occurredAt)
    {
        AggregateId = aggregateId;
        OccurredAt = occurredAt;
        Version = 1;
    }
}
