using MediatR;

namespace ReviewService.Domain.Events;

public sealed record ReviewSubmittedEvent(
    long ReviewSrlNum,
    long? FeedbackNum,
    char Status) : INotification;

public sealed record ReviewStatusChangedEvent(
    long ReviewSrlNum,
    char NewStatus) : INotification;

public sealed record ReviewDetailAddedEvent(
    long ReviewMainSrl,
    long ReviewNum,
    long ReviewedBy) : INotification;

public sealed record FeedbackSubmittedEvent(
    long CourseId,
    string UserId,
    DateTime ReviewDate) : INotification;

public sealed record FeedbackCancelledEvent(
    long CourseId,
    string UserId,
    string CancellationReason) : INotification;
