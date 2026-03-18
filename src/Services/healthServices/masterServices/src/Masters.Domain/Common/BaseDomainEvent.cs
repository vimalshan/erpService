namespace Masters.Domain.Common;

public abstract class BaseDomainEvent : IDomainEvent
{
    protected BaseDomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
        EventType = GetType().Name;
    }

    public DateTime OccurredOn { get; }
    public string EventType { get; }
}
