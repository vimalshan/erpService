namespace ProxyModule.Domain.Events;

public sealed class ProxyRightUpdatedEvent : IDomainEvent
{
    public long ProxyId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProxyRightUpdatedEvent(long proxyId)
    {
        ProxyId = proxyId;
    }
}
