using WMTransactional.Domain.Common;

namespace WMTransactional.Domain.Events;

public sealed record ReceivingCreatedEvent(string ReceivingNumber, int PoId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ReceivingClosedEvent(string ReceivingNumber, int PoId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ReceivingCancelledEvent(string ReceivingNumber, int PoId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
