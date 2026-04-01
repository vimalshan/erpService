using LetTransactionService.Domain.Common;

namespace LetTransactionService.Domain.Events;

public sealed record FeedbackSubmittedEvent(long FeedbackNumber, long NominationNumber) : DomainEvent;
public sealed record FeedbackCancelledEvent(long FeedbackNumber, string? CancelRemark) : DomainEvent;
public sealed record FeedbackRatingUpdatedEvent(long FeedbackNumber, long OverallRating) : DomainEvent;
