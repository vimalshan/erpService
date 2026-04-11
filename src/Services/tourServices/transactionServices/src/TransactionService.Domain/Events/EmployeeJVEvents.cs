using TransactionService.Domain.Common;

namespace TransactionService.Domain.Events;

public sealed record EmployeeJVCreatedEvent(
    Guid EventId,
    long JvBatchId,
    long EmployeeSysId,
    string JvType,
    decimal NetAmount,
    long CreatedBy,
    DateTime OccurredOn) : IDomainEvent;

public sealed record EmployeeJVPostedEvent(
    Guid EventId,
    long JvBatchId,
    string? OracleRefNo,
    long PostedBy,
    DateTime OccurredOn) : IDomainEvent;

public sealed record EmployeeJVReversedEvent(
    Guid EventId,
    long JvBatchId,
    long ReversedBy,
    DateTime OccurredOn) : IDomainEvent;
