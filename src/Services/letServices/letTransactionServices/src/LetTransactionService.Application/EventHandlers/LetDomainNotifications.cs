using LetTransactionService.Application.Interfaces;
using LetTransactionService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LetTransactionService.Application.EventHandlers;

// Domain notification wrappers
public record LetRequestCreatedNotification(LetRequestCreatedEvent Event) : INotification;
public record FeedbackSubmittedNotification(FeedbackSubmittedEvent Event) : INotification;
public record ReviewCreatedNotification(ReviewCreatedEvent Event) : INotification;
public record ReviewApprovedNotification(ReviewApprovedEvent Event) : INotification;

// Handlers
public class LetRequestCreatedHandler(
    IMessagePublisher publisher,
    ILogger<LetRequestCreatedHandler> logger)
    : INotificationHandler<LetRequestCreatedNotification>
{
    public async Task Handle(LetRequestCreatedNotification notification, CancellationToken ct)
    {
        logger.LogInformation("LET request created: {RequestNumber} by {Employee}",
            notification.Event.RequestNumber, notification.Event.EmployeeUserId);

        await publisher.PublishAsync(notification.Event, "let.request.created", ct);
    }
}

public class FeedbackSubmittedHandler(
    IMessagePublisher publisher,
    ILogger<FeedbackSubmittedHandler> logger)
    : INotificationHandler<FeedbackSubmittedNotification>
{
    public async Task Handle(FeedbackSubmittedNotification notification, CancellationToken ct)
    {
        logger.LogInformation("Feedback submitted: {FeedbackNumber} for nomination {NominationNumber}",
            notification.Event.FeedbackNumber, notification.Event.NominationNumber);

        await publisher.PublishAsync(notification.Event, "let.feedback.submitted", ct);
    }
}

public class ReviewCreatedHandler(
    IMessagePublisher publisher,
    ILogger<ReviewCreatedHandler> logger)
    : INotificationHandler<ReviewCreatedNotification>
{
    public async Task Handle(ReviewCreatedNotification notification, CancellationToken ct)
    {
        logger.LogInformation("Review created: {ReviewSerial} for feedback {FeedbackNumber}",
            notification.Event.ReviewSerialNumber, notification.Event.FeedbackNumber);

        await publisher.PublishAsync(notification.Event, "let.review.created", ct);
    }
}

public class ReviewApprovedHandler(
    IMessagePublisher publisher,
    ILogger<ReviewApprovedHandler> logger)
    : INotificationHandler<ReviewApprovedNotification>
{
    public async Task Handle(ReviewApprovedNotification notification, CancellationToken ct)
    {
        logger.LogInformation("Review approved: {ReviewSerial}", notification.Event.ReviewSerialNumber);

        await publisher.PublishAsync(notification.Event, "let.review.approved", ct);
    }
}
