using CardManagement.Domain.Common;

namespace CardManagement.Domain.Events;

public sealed record CardMapCreatedEvent(decimal SysId, long CanteenUnit, string CardNumber) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record CardMapClosedEvent(decimal SysId, long CanteenUnit, string CardNumber) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record GuestCardCreatedEvent(long CanteenUnit, long CardSequence, string? CardNumber, string? CardName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record GuestCardUpdatedEvent(long CanteenUnit, long CardSequence, string? CardNumber) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record GuestCardClosedEvent(long CanteenUnit, long CardSequence, string? CardNumber) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record CardSettledEvent(decimal SysId, long CanteenUnit, string CardNumber, DateTime SettlementDate) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
