namespace WebsiteContentService.Domain.Common;

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
