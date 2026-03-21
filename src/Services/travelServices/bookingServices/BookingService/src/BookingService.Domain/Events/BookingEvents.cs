using BookingService.Domain.Common;
using BookingService.Domain.Enums;
using BookingService.Domain.ValueObjects;

namespace BookingService.Domain.Events;

public sealed record BookingCreatedEvent(
    long BookingNumber,
    string UserCode,
    BookingType BookingType,
    DateRange TravelDates) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record BookingConfirmedEvent(
    long BookingNumber,
    long ConfirmationNumber) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record BookingCancelledEvent(
    long BookingNumber,
    string CancelledBy,
    string Remarks) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record CouponIssuedEvent(
    long CouponId,
    int NumberOfTickets,
    string? AirlineName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
