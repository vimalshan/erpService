namespace ReimbursementService.Infrastructure.Messaging.Contracts;

/// <summary>
/// Plain message contracts for RabbitMQ — separate from domain events (no constructor params).
/// </summary>

public sealed record ReimbursementSubmittedMessage
{
    public long ReimId { get; init; }
    public string RefNo { get; init; } = default!;
    public long EmpSysId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "INR";
    public DateTime OccurredOn { get; init; }
}

public sealed record ReimbursementApprovedMessage
{
    public long ReimId { get; init; }
    public string RefNo { get; init; } = default!;
    public long ApprovedBy { get; init; }
    public int ApprovalLevel { get; init; }
    public DateTime OccurredOn { get; init; }
}

public sealed record ReimbursementPaidMessage
{
    public long ReimId { get; init; }
    public string RefNo { get; init; } = default!;
    public DateOnly? PaymentDate { get; init; }
    public DateTime OccurredOn { get; init; }
}
