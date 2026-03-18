using BookingService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingService.Infrastructure.Messaging.Consumers;

/// <summary>
/// Handles domain events raised when a booking is created and publishes to RabbitMQ.
/// </summary>
public class BookingCreatedEventHandler(IMessagePublisher publisher, ILogger<BookingCreatedEventHandler> logger)
    : INotificationHandler<BookingCreatedDomainEvent>
{
    public async Task Handle(BookingCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Publishing BookingCreated event for {AppNo}", notification.BookingAppNo);
        await publisher.PublishAsync("booking.created", notification, cancellationToken);
    }
}

/// <summary>
/// Handles domain events raised when a booking status changes and publishes to RabbitMQ.
/// </summary>
public class BookingStatusChangedEventHandler(IMessagePublisher publisher, ILogger<BookingStatusChangedEventHandler> logger)
    : INotificationHandler<BookingStatusChangedDomainEvent>
{
    public async Task Handle(BookingStatusChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Publishing BookingStatusChanged event for BookingId={Id} New={Status}",
            notification.BookingId, notification.NewStatus);
        await publisher.PublishAsync($"booking.status.{notification.NewStatus.ToLower()}", notification, cancellationToken);
    }
}

/// <summary>
/// Handles domain events raised when an attendee is registered and publishes to RabbitMQ.
/// </summary>
public class AttendeeRegisteredEventHandler(IMessagePublisher publisher, ILogger<AttendeeRegisteredEventHandler> logger)
    : INotificationHandler<AttendeeRegisteredDomainEvent>
{
    public async Task Handle(AttendeeRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Publishing AttendeeRegistered event BookingId={BookingId} Attendee={AttendeeSysId}",
            notification.BookingId, notification.AttendeeSysId);
        await publisher.PublishAsync("booking.attendee.registered", notification, cancellationToken);
    }
}
