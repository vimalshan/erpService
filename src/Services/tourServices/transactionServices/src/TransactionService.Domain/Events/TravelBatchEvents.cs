using TransactionService.Domain.Common;

namespace TransactionService.Domain.Events;

public sealed record TravelBatchCreatedEvent(
    Guid EventId,
    string BatchId,
    string VendorId,
    string AdminId,
    DateTime OccurredOn) : IDomainEvent;

public sealed record TravelBatchAdminApprovedEvent(
    Guid EventId,
    string BatchId,
    string ApprovedBy,
    DateTime OccurredOn) : IDomainEvent;

public sealed record TravelBatchFinanceApprovedEvent(
    Guid EventId,
    string BatchId,
    string ApprovedBy,
    DateTime OccurredOn) : IDomainEvent;

public sealed record TravelBatchJVPostedEvent(
    Guid EventId,
    string BatchId,
    string JvId,
    DateTime OccurredOn) : IDomainEvent;

public sealed record TravelBatchRejectedEvent(
    Guid EventId,
    string BatchId,
    string RejectedBy,
    string? Remarks,
    DateTime OccurredOn) : IDomainEvent;
