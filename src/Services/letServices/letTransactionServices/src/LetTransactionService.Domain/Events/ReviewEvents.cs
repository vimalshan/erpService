using LetTransactionService.Domain.Common;

namespace LetTransactionService.Domain.Events;

public sealed record ReviewCreatedEvent(long ReviewSerialNumber, long FeedbackNumber) : DomainEvent;
public sealed record ReviewApprovedEvent(long ReviewSerialNumber) : DomainEvent;
public sealed record ReviewSubAddedEvent(long ReviewSerialNumber, long ReviewNumber) : DomainEvent;
