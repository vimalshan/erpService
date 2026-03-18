using BookingService.Domain.Common;

namespace BookingService.Domain.Events;

public sealed record BookingStatusChangedDomainEvent(
    long BookingId,
    string BookingAppNo,
    string PreviousStatus,
    string NewStatus,
    long ChangedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
