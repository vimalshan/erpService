using ReimbursementService.Domain.Common;
using ReimbursementService.Domain.Entities;

namespace ReimbursementService.Domain.Events;

public sealed class ReimbursementCreatedEvent(ReimbursementTransaction reimbursement) : BaseEvent
{
    public ReimbursementTransaction Reimbursement { get; } = reimbursement;
}

public sealed class ReimbursementSubmittedEvent(ReimbursementTransaction reimbursement) : BaseEvent
{
    public ReimbursementTransaction Reimbursement { get; } = reimbursement;
    public string RefNo { get; } = reimbursement.ReimRefNo;
}

public sealed class ReimbursementApprovedEvent(ReimbursementTransaction reimbursement) : BaseEvent
{
    public ReimbursementTransaction Reimbursement { get; } = reimbursement;
    public long ApprovedBy { get; } = reimbursement.ApprovedBy!.Value;
}

public sealed class ReimbursementRejectedEvent(ReimbursementTransaction reimbursement) : BaseEvent
{
    public ReimbursementTransaction Reimbursement { get; } = reimbursement;
    public string? Reason { get; } = reimbursement.RejectionReason;
}

public sealed class ReimbursementPaidEvent(ReimbursementTransaction reimbursement) : BaseEvent
{
    public ReimbursementTransaction Reimbursement { get; } = reimbursement;
    public DateOnly? PaymentDate { get; } = reimbursement.PaymentDate;
}
