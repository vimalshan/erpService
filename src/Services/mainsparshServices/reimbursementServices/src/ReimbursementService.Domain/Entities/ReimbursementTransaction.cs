using ReimbursementService.Domain.Common;
using ReimbursementService.Domain.Enums;
using ReimbursementService.Domain.Events;
using ReimbursementService.Domain.ValueObjects;

namespace ReimbursementService.Domain.Entities;

/// <summary>
/// Reimbursement Transaction entity — aggregate root for REIM_TRAN.
/// </summary>
public sealed class ReimbursementTransaction : BaseEntity
{
    public long ReimId { get; private set; }
    public string ReimRefNo { get; private set; } = default!;
    public long EmpSysId { get; private set; }
    public ReimbursementType ReimType { get; private set; }
    public Money Amount { get; private set; } = default!;
    public DateOnly ReimDate { get; private set; }
    public DateOnly ExpenseDate { get; private set; }
    public string? Description { get; private set; }
    public string? Location { get; private set; }
    public ReimbursementStatus Status { get; private set; } = ReimbursementStatus.Draft;
    public int? ApprovalLevel { get; private set; }
    public long? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public string? RejectionReason { get; private set; }
    public DateOnly? PaymentDate { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    // EF constructor
    private ReimbursementTransaction() { }

    public static ReimbursementTransaction Create(
        string refNo,
        long empSysId,
        ReimbursementType type,
        Money amount,
        DateOnly reimDate,
        DateOnly expenseDate,
        string? description,
        string? location,
        long createdBy)
    {
        var entity = new ReimbursementTransaction
        {
            ReimRefNo = refNo,
            EmpSysId = empSysId,
            ReimType = type,
            Amount = amount,
            ReimDate = reimDate,
            ExpenseDate = expenseDate,
            Description = description,
            Location = location,
            Status = ReimbursementStatus.Draft,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        entity.AddDomainEvent(new ReimbursementCreatedEvent(entity));
        return entity;
    }

    public void Submit()
    {
        if (Status != ReimbursementStatus.Draft)
            throw new InvalidOperationException($"Cannot submit a reimbursement in {Status} status.");
        Status = ReimbursementStatus.Submitted;
        AddDomainEvent(new ReimbursementSubmittedEvent(this));
    }

    public void Approve(long approvedBy, int approvalLevel)
    {
        if (Status != ReimbursementStatus.Submitted)
            throw new InvalidOperationException($"Cannot approve a reimbursement in {Status} status.");
        Status = ReimbursementStatus.Approved;
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovalLevel = approvalLevel;
        AddDomainEvent(new ReimbursementApprovedEvent(this));
    }

    public void Reject(long rejectedBy, string reason)
    {
        if (Status != ReimbursementStatus.Submitted)
            throw new InvalidOperationException($"Cannot reject a reimbursement in {Status} status.");
        Status = ReimbursementStatus.Rejected;
        RejectionReason = reason;
        UpdatedBy = rejectedBy;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new ReimbursementRejectedEvent(this));
    }

    public void MarkAsPaid(DateOnly paymentDate, long updatedBy)
    {
        if (Status != ReimbursementStatus.Approved)
            throw new InvalidOperationException($"Cannot mark as paid a reimbursement in {Status} status.");
        Status = ReimbursementStatus.Paid;
        PaymentDate = paymentDate;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new ReimbursementPaidEvent(this));
    }

    public void Update(
        ReimbursementType type,
        Money amount,
        DateOnly reimDate,
        DateOnly expenseDate,
        string? description,
        string? location,
        long updatedBy)
    {
        if (Status != ReimbursementStatus.Draft)
            throw new InvalidOperationException("Only DRAFT reimbursements can be updated.");
        ReimType = type;
        Amount = amount;
        ReimDate = reimDate;
        ExpenseDate = expenseDate;
        Description = description;
        Location = location;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
