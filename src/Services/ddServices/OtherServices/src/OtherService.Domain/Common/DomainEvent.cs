namespace OtherService.Domain.Common;

public abstract class DomainEvent
{
    protected DomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
        EventId = Guid.NewGuid();
    }

    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}
