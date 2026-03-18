namespace ProxyModule.Domain.Events;

public sealed class ProxyRightCreatedEvent : IDomainEvent
{
    public long ProxyId { get; }
    public long ProxyUserId { get; }
    public long DelegatedUserId { get; }
    public string ProxyType { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProxyRightCreatedEvent(long proxyId, long proxyUserId, long delegatedUserId, string proxyType)
    {
        ProxyId = proxyId;
        ProxyUserId = proxyUserId;
        DelegatedUserId = delegatedUserId;
        ProxyType = proxyType;
    }
}
