using MediatR;

namespace BookingService.Domain.Events;

public record BookingCreatedEvent(string BookingId, string EmployeeSysId, string Type) : INotification;
public record BookingConfirmedEvent(string BookingId, string ConfirmationId) : INotification;
public record BookingCancelledEvent(string BookingId, string Reason) : INotification;
public record BookingApprovedEvent(string BookingId, string ApprovedBy) : INotification;
public record TicketBookedEvent(string TicketId, string BookingId, string StartCity, string EndCity) : INotification;
public record StayBookedEvent(string StayId, string BookingId, string City) : INotification;
public record CabBookedEvent(string CabId, string BookingId, string PickupLocation) : INotification;
