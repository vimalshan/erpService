using WMTransactional.Domain.Common;

namespace WMTransactional.Domain.Events;

public sealed record ShipmentCreatedEvent(string ShipmentNumber, int SoId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ShipmentShippedEvent(string ShipmentNumber, int SoId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ShipmentCancelledEvent(string ShipmentNumber, int SoId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
