namespace TransactionService.Domain.Events;

using TransactionService.Domain.Common;

public sealed record RequestCreatedEvent(
    long RequestId, long RequestedBy, long? LocationId, DateTime OccurredOn) : IDomainEvent;

public sealed record RequestApprovedEvent(
    long RequestSubId, long RequestId, long ApprovedQty,
    long ApproverSysId, DateTime OccurredOn) : IDomainEvent;

public sealed record RequestFullyProcessedEvent(
    long RequestId, long RequestedBy, DateTime OccurredOn) : IDomainEvent;

public sealed record OrderCreatedEvent(
    long OrderMainId, long VendorId, long LocationId, DateTime OccurredOn) : IDomainEvent;

public sealed record OrderReceivedEvent(
    long OrderMainId, long OrderSubId, long ReceivedQty,
    long ReceivedBy, DateTime OccurredOn) : IDomainEvent;

public sealed record BudgetExceededEvent(
    long LocationId, long DeptId, long FinYearId,
    long BudgetAmount, long RequestedAmount, DateTime OccurredOn) : IDomainEvent;

public sealed record BudgetAllocatedEvent(
    long LocationId, long DeptId, long FinYearId,
    long Amount, DateTime OccurredOn) : IDomainEvent;
