using BookingService.Domain.Common;

namespace BookingService.Domain.Events;

public sealed record AttendeeRegisteredDomainEvent(
    long BookingId,
    long AttendeeSysId,
    int Serial) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
