namespace ProxyModule.Domain.Events;

public sealed class ProxyRightDeactivatedEvent : IDomainEvent
{
    public long ProxyId { get; }
    public long ProxyUserId { get; }
    public long DelegatedUserId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProxyRightDeactivatedEvent(long proxyId, long proxyUserId, long delegatedUserId)
    {
        ProxyId = proxyId;
        ProxyUserId = proxyUserId;
        DelegatedUserId = delegatedUserId;
    }
}
