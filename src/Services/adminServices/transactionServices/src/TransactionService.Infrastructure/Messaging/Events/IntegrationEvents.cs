namespace TransactionService.Infrastructure.Messaging.Events;

public sealed record RequestCreatedIntegrationEvent(
    long RequestId, long RequestedBy, long? LocationId, DateTime OccurredOn);

public sealed record RequestApprovedIntegrationEvent(
    long RequestSubId, long RequestId, long ApprovedQty,
    long ApproverSysId, DateTime OccurredOn);

public sealed record OrderCreatedIntegrationEvent(
    long OrderMainId, long VendorId, long LocationId, DateTime OccurredOn);

public sealed record OrderReceivedIntegrationEvent(
    long OrderMainId, long OrderSubId, long ReceivedQty,
    long ReceivedBy, DateTime OccurredOn);

public sealed record BudgetExceededIntegrationEvent(
    long LocationId, long DeptId, long FinYearId,
    long BudgetAmount, long RequestedAmount, DateTime OccurredOn);
