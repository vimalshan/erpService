using ReceivingService.Domain.Common;
using ReceivingService.Domain.Entities;

namespace ReceivingService.Domain.Events;

public sealed record ReceivingCreatedEvent(Receiving Receiving) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ReceivingClosedEvent(Receiving Receiving) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ReceivingCancelledEvent(Receiving Receiving) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
