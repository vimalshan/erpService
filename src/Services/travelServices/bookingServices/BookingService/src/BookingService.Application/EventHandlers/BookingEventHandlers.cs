using MediatR;
using BookingService.Domain.Common;
using BookingService.Domain.Events;
using BookingService.Domain.Interfaces;
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
    private readonly IMessagePublisher _publisher;

    public BookingCreatedEventHandler(ILogger<BookingCreatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(BookingCreatedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        _logger.LogInformation(
            "Booking created: #{BookingNumber} by {UserCode} for {BookingType} from {From} to {To}",
            ev.BookingNumber, ev.UserCode, ev.BookingType,
            ev.TravelDates.From, ev.TravelDates.To);
        await _publisher.PublishAsync("booking.created", ev, cancellationToken);
    }
}

public class BookingConfirmedEventHandler : INotificationHandler<BookingConfirmedNotification>
{
    private readonly ILogger<BookingConfirmedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public BookingConfirmedEventHandler(ILogger<BookingConfirmedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(BookingConfirmedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        _logger.LogInformation(
            "Booking #{BookingNumber} confirmed with confirmation #{ConfirmationNumber}",
            ev.BookingNumber, ev.ConfirmationNumber);
        await _publisher.PublishAsync("booking.confirmed", ev, cancellationToken);
    }
}

public class BookingCancelledEventHandler : INotificationHandler<BookingCancelledNotification>
{
    private readonly ILogger<BookingCancelledEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public BookingCancelledEventHandler(ILogger<BookingCancelledEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(BookingCancelledNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        _logger.LogInformation(
            "Booking #{BookingNumber} cancelled by {CancelledBy}: {Remarks}",
            ev.BookingNumber, ev.CancelledBy, ev.Remarks);
        await _publisher.PublishAsync("booking.cancelled", ev, cancellationToken);
    }
}
