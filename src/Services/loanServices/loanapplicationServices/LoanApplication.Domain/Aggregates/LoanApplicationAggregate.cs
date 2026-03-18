using LoanApplication.Domain.Common;
using LoanApplication.Domain.Events;
using LoanApplication.Domain.ValueObjects;

namespace LoanApplication.Domain.Aggregates;

/// <summary>
/// Loan Application Aggregate Root
/// </summary>
public class LoanApplicationAggregate : Entity
{
    public long EmployeeId { get; private set; }
    public long LoanId { get; private set; }
    public long AppliedBy { get; private set; }
    public DateTime AppliedOn { get; private set; }
    public LoanSource Source { get; private set; } = null!;
    public Money Amount { get; private set; } = null!;
    public long? SubclassId { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public LoanApplicationStatus Status { get; private set; } = null!;
    public long GuarantorId { get; private set; }
    public long? SecondGuarantorId { get; private set; }
    public string? ApprovalRemarks { get; private set; }
    public long? RequiredBy { get; private set; }
    public long? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public int? TenureMonths { get; private set; }
    public char? SpecialSanction { get; private set; }

    private LoanApplicationAggregate() { }

    public static LoanApplicationAggregate Create(
        long employeeId,
        long loanId,
        long appliedBy,
        LoanSource source,
        Money amount,
        string reason,
        long guarantorId,
        int tenureMonths)
    {
        // Validate
        if (employeeId <= 0)
            throw new ArgumentException("Employee ID must be greater than 0", nameof(employeeId));
        
        if (loanId <= 0)
            throw new ArgumentException("Loan ID must be greater than 0", nameof(loanId));
        
        if (amount == null || amount.IsNegative)
            throw new ArgumentException("Amount must be positive", nameof(amount));
        
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required", nameof(reason));
        
        if (reason.Length > 200)
            throw new ArgumentException("Reason cannot exceed 200 characters", nameof(reason));
        
        if (guarantorId <= 0)
            throw new ArgumentException("Guarantor ID must be greater than 0", nameof(guarantorId));
        
        if (employeeId == guarantorId)
            throw new ArgumentException("Guarantor cannot be the same as applicant");
        
        if (tenureMonths <= 0)
            throw new ArgumentException("Tenure must be greater than 0", nameof(tenureMonths));

        var now = DateTime.UtcNow;

        var aggregate = new LoanApplicationAggregate
        {
            EmployeeId = employeeId,
            LoanId = loanId,
            AppliedBy = appliedBy,
            AppliedOn = now,
            Source = source,
            Amount = amount,
            Reason = reason,
            Status = LoanApplicationStatus.CreateNew(),
            GuarantorId = guarantorId,
            TenureMonths = tenureMonths,
            RequiredBy = employeeId,
            CreatedAt = now,
            CreatedBy = appliedBy,
            ModifiedAt = now,
            ModifiedBy = appliedBy
        };

        // Raise domain event
        aggregate.RaiseDomainEvent(new LoanApplicationCreatedEvent
        {
            LoanApplicationId = aggregate.Id,
            EmployeeId = employeeId,
            LoanId = loanId,
            Amount = amount.Amount,
            Reason = reason,
            CreatedAt = now
        });

        return aggregate;
    }

    /// <summary>
    /// Submit the loan application for approval
    /// </summary>
    public void Submit(long submittedBy)
    {
        if (Status.IsApplied)
            throw new InvalidOperationException("Loan application is already submitted");

        if (!Status.IsCreated)
            throw new InvalidOperationException("Only created applications can be submitted");

        Status = LoanApplicationStatus.Apply();
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = submittedBy;

        RaiseDomainEvent(new LoanApplicationSubmittedEvent
        {
            LoanApplicationId = Id,
            EmployeeId = EmployeeId,
            SubmittedAt = ModifiedAt
        });
    }

    /// <summary>
    /// Approve the loan application
    /// </summary>
    public void Approve(long approvedBy, string? remarks = null)
    {
        if (!Status.IsApplied && !Status.IsCreated)
            throw new InvalidOperationException("Only pending applications can be approved");

        if (approvedBy <= 0)
            throw new ArgumentException("Approver ID must be greater than 0", nameof(approvedBy));

        Status = LoanApplicationStatus.Approve();
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovalRemarks = remarks;
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = approvedBy;

        RaiseDomainEvent(new LoanApplicationApprovedEvent
        {
            LoanApplicationId = Id,
            ApprovedBy = approvedBy,
            ApprovedAt = ApprovedOn.Value,
            Remarks = remarks
        });
    }

    /// <summary>
    /// Reject the loan application
    /// </summary>
    public void Reject(long rejectedBy, string? remarks = null)
    {
        if (!Status.IsApplied && !Status.IsCreated)
            throw new InvalidOperationException("Only pending applications can be rejected");

        if (rejectedBy <= 0)
            throw new ArgumentException("Rejector ID must be greater than 0", nameof(rejectedBy));

        Status = LoanApplicationStatus.Reject();
        ApprovalRemarks = remarks;
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = rejectedBy;

        RaiseDomainEvent(new LoanApplicationRejectedEvent
        {
            LoanApplicationId = Id,
            RejectedBy = rejectedBy,
            RejectedAt = ModifiedAt,
            Remarks = remarks
        });
    }

    /// <summary>
    /// Disburse the loan
    /// </summary>
    public void Disburse(long disbursingBy)
    {
        if (!Status.IsApproved)
            throw new InvalidOperationException("Only approved applications can be disbursed");

        if (disbursingBy <= 0)
            throw new ArgumentException("Disbursing user ID must be greater than 0", nameof(disbursingBy));

        Status = LoanApplicationStatus.Disburse();
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = disbursingBy;

        RaiseDomainEvent(new LoanApplicationDisbursedEvent
        {
            LoanApplicationId = Id,
            DisbursedAmount = Amount.Amount,
            DisbursedAt = ModifiedAt
        });
    }

    /// <summary>
    /// Set second guarantor
    /// </summary>
    public void SetSecondGuarantor(long secondGuarantorId, long modifiedBy)
    {
        if (secondGuarantorId <= 0)
            throw new ArgumentException("Second guarantor ID must be greater than 0", nameof(secondGuarantorId));

        if (secondGuarantorId == EmployeeId || secondGuarantorId == GuarantorId)
            throw new ArgumentException("Second guarantor must be different from applicant and first guarantor");

        SecondGuarantorId = secondGuarantorId;
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    /// <summary>
    /// Mark for special sanction
    /// </summary>
    public void MarkForSpecialSanction(bool sanctioned, long modifiedBy)
    {
        SpecialSanction = sanctioned ? 'Y' : 'N';
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }
}
