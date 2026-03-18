using MediatR;
using Microsoft.Extensions.Logging;
using ReviewService.Domain.Events;

namespace ReviewService.Infrastructure.EventHandlers;

public class FeedbackSubmittedEventHandler : INotificationHandler<FeedbackSubmittedEvent>
{
    private readonly ILogger<FeedbackSubmittedEventHandler> _logger;

    public FeedbackSubmittedEventHandler(ILogger<FeedbackSubmittedEventHandler> logger)
        => _logger = logger;

    public Task Handle(FeedbackSubmittedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Feedback submitted for Course {CourseId} by User {UserId} on {ReviewDate}",
            notification.CourseId, notification.UserId, notification.ReviewDate);

        return Task.CompletedTask;
    }
}

public class ReviewSubmittedEventHandler : INotificationHandler<ReviewSubmittedEvent>
{
    private readonly ILogger<ReviewSubmittedEventHandler> _logger;

    public ReviewSubmittedEventHandler(ILogger<ReviewSubmittedEventHandler> logger)
        => _logger = logger;

    public Task Handle(ReviewSubmittedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Review {ReviewSrlNum} submitted with status {Status}",
            notification.ReviewSrlNum, notification.Status);

        return Task.CompletedTask;
    }
}

public class ReviewStatusChangedEventHandler : INotificationHandler<ReviewStatusChangedEvent>
{
    private readonly ILogger<ReviewStatusChangedEventHandler> _logger;

    public ReviewStatusChangedEventHandler(ILogger<ReviewStatusChangedEventHandler> logger)
        => _logger = logger;

    public Task Handle(ReviewStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Review {ReviewSrlNum} status changed to {NewStatus}",
            notification.ReviewSrlNum, notification.NewStatus);

        return Task.CompletedTask;
    }
}
