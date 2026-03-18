using BookingService.Domain.Common;

namespace BookingService.Domain.Events;

public sealed record BookingCreatedDomainEvent(
    string BookingAppNo,
    long CreatedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
