using MediatR;
using BookingService.Domain.Events;
using BookingService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookingService.Application.Handlers;

public class BookingCreatedEventHandler(IMessagePublisher publisher, ILogger<BookingCreatedEventHandler> logger)
    : INotificationHandler<BookingCreatedEvent>
{
    public async Task Handle(BookingCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event: Booking {BookingId} created for employee {EmployeeId}",
            notification.BookingId, notification.EmployeeSysId);
        await publisher.PublishAsync(notification, "booking.events", "booking.created", ct);
    }
}

public class BookingConfirmedEventHandler(IMessagePublisher publisher, ILogger<BookingConfirmedEventHandler> logger)
    : INotificationHandler<BookingConfirmedEvent>
{
    public async Task Handle(BookingConfirmedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event: Booking {BookingId} confirmed with {ConfirmationId}",
            notification.BookingId, notification.ConfirmationId);
        await publisher.PublishAsync(notification, "booking.events", "booking.confirmed", ct);
    }
}

public class BookingCancelledEventHandler(IMessagePublisher publisher, ILogger<BookingCancelledEventHandler> logger)
    : INotificationHandler<BookingCancelledEvent>
{
    public async Task Handle(BookingCancelledEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event: Booking {BookingId} cancelled: {Reason}",
            notification.BookingId, notification.Reason);
        await publisher.PublishAsync(notification, "booking.events", "booking.cancelled", ct);
    }
}

public class BookingApprovedEventHandler(IMessagePublisher publisher, ILogger<BookingApprovedEventHandler> logger)
    : INotificationHandler<BookingApprovedEvent>
{
    public async Task Handle(BookingApprovedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event: Booking {BookingId} approved by {ApprovedBy}",
            notification.BookingId, notification.ApprovedBy);
        await publisher.PublishAsync(notification, "booking.events", "booking.approved", ct);
    }
}
