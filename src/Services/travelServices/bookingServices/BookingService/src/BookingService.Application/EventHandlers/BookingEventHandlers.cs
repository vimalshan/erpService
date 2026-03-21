using MediatR;
using BookingService.Domain.Common;
using BookingService.Domain.Events;
using Microsoft.Extensions.Logging;

namespace BookingService.Application.EventHandlers;

// ── Notification wrappers ────────────────────────────────────────────────────
// Domain events don't depend on MediatR; these thin wrappers bridge the gap.
public record BookingCreatedNotification(BookingCreatedEvent DomainEvent) : INotification;
public record BookingConfirmedNotification(BookingConfirmedEvent DomainEvent) : INotification;
public record BookingCancelledNotification(BookingCancelledEvent DomainEvent) : INotification;

// ── Handlers ─────────────────────────────────────────────────────────────────
public class BookingCreatedEventHandler : INotificationHandler<BookingCreatedNotification>
{
    private readonly ILogger<BookingCreatedEventHandler> _logger;
    public BookingCreatedEventHandler(ILogger<BookingCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(BookingCreatedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        _logger.LogInformation(
            "Booking created: #{BookingNumber} by {UserCode} for {BookingType} from {From} to {To}",
            ev.BookingNumber, ev.UserCode, ev.BookingType,
            ev.TravelDates.From, ev.TravelDates.To);
        return Task.CompletedTask;
    }
}

public class BookingConfirmedEventHandler : INotificationHandler<BookingConfirmedNotification>
{
    private readonly ILogger<BookingConfirmedEventHandler> _logger;
    public BookingConfirmedEventHandler(ILogger<BookingConfirmedEventHandler> logger) => _logger = logger;

    public Task Handle(BookingConfirmedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        _logger.LogInformation(
            "Booking #{BookingNumber} confirmed with confirmation #{ConfirmationNumber}",
            ev.BookingNumber, ev.ConfirmationNumber);
        return Task.CompletedTask;
    }
}

public class BookingCancelledEventHandler : INotificationHandler<BookingCancelledNotification>
{
    private readonly ILogger<BookingCancelledEventHandler> _logger;
    public BookingCancelledEventHandler(ILogger<BookingCancelledEventHandler> logger) => _logger = logger;

    public Task Handle(BookingCancelledNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        _logger.LogInformation(
            "Booking #{BookingNumber} cancelled by {CancelledBy}: {Remarks}",
            ev.BookingNumber, ev.CancelledBy, ev.Remarks);
        return Task.CompletedTask;
    }
}
