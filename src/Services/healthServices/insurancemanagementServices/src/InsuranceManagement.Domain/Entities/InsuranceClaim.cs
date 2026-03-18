using InsuranceManagement.Domain.ValueObjects;
using InsuranceManagement.Domain.Common;

namespace InsuranceManagement.Domain.Entities;

/// <summary>
/// Insurance Claim entity representing claims submitted by employees
/// </summary>
public class InsuranceClaim : AggregateRoot
{
    public long ClaimId { get; private set; }
    public long EmpSysId { get; private set; }
    public long EnrollmentId { get; private set; }
    public long InsurancePlanId { get; private set; }
    public ClaimType ClaimType { get; private set; } = ClaimType.InPatient_Claim;
    public Money ClaimAmount { get; private set; } = Money.Zero;
    public Money ReimbursableAmount { get; private set; } = Money.Zero;
    public Money ApprovedAmount { get; private set; } = Money.Zero;
    public DateTime ServiceDate { get; private set; }
    public string HospitalName { get; private set; } = string.Empty;
    public string ClaimRemarks { get; private set; } = string.Empty;
    public ClaimStatus Status { get; private set; } = ClaimStatus.Submitted_Status;
    public string? RejectionReason { get; private set; }
    public DateTime? ApprovalDate { get; private set; }
    public long? ApprovedBy { get; private set; }
    public DateTime? PaidDate { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public long? ModifiedBy { get; private set; }

    // Navigation properties
    public InsuranceEnrollment? Enrollment { get; set; }

    // EF constructor
    private InsuranceClaim() { }

    public InsuranceClaim(
        long empSysId,
        long enrollmentId,
        long insurancePlanId,
        ClaimType claimType,
        Money claimAmount,
        Money reimbursableAmount,
        DateTime serviceDate,
        string hospitalName,
        string remarks,
        long createdBy)
    {
        if (empSysId <= 0)
            throw new ArgumentException("Employee ID must be greater than zero", nameof(empSysId));

        if (enrollmentId <= 0)
            throw new ArgumentException("Enrollment ID must be greater than zero", nameof(enrollmentId));

        if (insurancePlanId <= 0)
            throw new ArgumentException("Insurance Plan ID must be greater than zero", nameof(insurancePlanId));

        if (serviceDate > DateTime.UtcNow)
            throw new ArgumentException("Service date cannot be in the future", nameof(serviceDate));

        if (claimAmount.IsEqual(Money.Zero) || claimAmount.IsLessThan(Money.Zero))
            throw new ArgumentException("Claim amount must be greater than zero", nameof(claimAmount));

        EmpSysId = empSysId;
        EnrollmentId = enrollmentId;
        InsurancePlanId = insurancePlanId;
        ClaimType = claimType ?? throw new ArgumentNullException(nameof(claimType));
        ClaimAmount = claimAmount;
        ReimbursableAmount = reimbursableAmount;
        ApprovedAmount = Money.Zero;
        ServiceDate = serviceDate;
        HospitalName = hospitalName ?? string.Empty;
        ClaimRemarks = remarks ?? string.Empty;
        Status = ClaimStatus.Submitted_Status;
        CreatedOn = DateTime.UtcNow;
        CreatedBy = createdBy;

        // Raise domain event
        AddDomainEvent(new InsuranceClaimSubmittedDomainEvent(
            0, // Will be set after insert
            empSysId,
            enrollmentId,
            insurancePlanId,
            claimType.Value,
            claimAmount.Amount));
    }

    public void Approve(Money approvedAmount, long approvedBy)
    {
        if (!Status.IsSubmitted && !Status.IsPending)
            throw new InvalidOperationException("Can only approve submitted or pending claims");

        if (approvedAmount.IsGreaterThan(ReimbursableAmount))
            throw new InvalidOperationException("Approved amount cannot exceed reimbursable amount");

        ApprovedAmount = approvedAmount;
        Status = ClaimStatus.Approved_Status;
        ApprovalDate = DateTime.UtcNow;
        ApprovedBy = approvedBy;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = approvedBy;

        // Raise domain event
        AddDomainEvent(new InsuranceClaimApprovedDomainEvent(
            ClaimId,
            EmpSysId,
            approvedAmount.Amount,
            approvedBy));
    }

    public void Reject(string reason, long rejectedBy)
    {
        if (!Status.IsSubmitted && !Status.IsPending)
            throw new InvalidOperationException("Can only reject submitted or pending claims");

        Status = ClaimStatus.Rejected_Status;
        RejectionReason = reason;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = rejectedBy;

        // Raise domain event
        AddDomainEvent(new InsuranceClaimRejectedDomainEvent(
            ClaimId,
            EmpSysId,
            reason,
            rejectedBy));
    }

    public void MarkAsPaid(long paidBy)
    {
        if (!Status.IsApproved)
            throw new InvalidOperationException("Can only mark approved claims as paid");

        Status = ClaimStatus.Paid_Status;
        PaidDate = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = paidBy;

        // Raise domain event
        AddDomainEvent(new InsuranceClaimPaidDomainEvent(
            ClaimId,
            EmpSysId,
            ApprovedAmount.Amount,
            PaidDate.Value));
    }
}

/// <summary>
/// Domain event raised when claim is submitted
/// </summary>
public class InsuranceClaimSubmittedDomainEvent : DomainEvent
{
    public long ClaimId { get; }
    public long EmpSysId { get; }
    public long EnrollmentId { get; }
    public long InsurancePlanId { get; }
    public string ClaimType { get; }
    public decimal ClaimAmount { get; }

    public InsuranceClaimSubmittedDomainEvent(long claimId, long empSysId, long enrollmentId, 
        long insurancePlanId, string claimType, decimal claimAmount)
    {
        ClaimId = claimId;
        EmpSysId = empSysId;
        EnrollmentId = enrollmentId;
        InsurancePlanId = insurancePlanId;
        ClaimType = claimType;
        ClaimAmount = claimAmount;
    }
}

/// <summary>
/// Domain event raised when claim is approved
/// </summary>
public class InsuranceClaimApprovedDomainEvent : DomainEvent
{
    public long ClaimId { get; }
    public long EmpSysId { get; }
    public decimal ApprovedAmount { get; }
    public long ApprovedBy { get; }

    public InsuranceClaimApprovedDomainEvent(long claimId, long empSysId, decimal approvedAmount, long approvedBy)
    {
        ClaimId = claimId;
        EmpSysId = empSysId;
        ApprovedAmount = approvedAmount;
        ApprovedBy = approvedBy;
    }
}

/// <summary>
/// Domain event raised when claim is rejected
/// </summary>
public class InsuranceClaimRejectedDomainEvent : DomainEvent
{
    public long ClaimId { get; }
    public long EmpSysId { get; }
    public string RejectionReason { get; }
    public long RejectedBy { get; }

    public InsuranceClaimRejectedDomainEvent(long claimId, long empSysId, string reason, long rejectedBy)
    {
        ClaimId = claimId;
        EmpSysId = empSysId;
        RejectionReason = reason;
        RejectedBy = rejectedBy;
    }
}

/// <summary>
/// Domain event raised when claim is paid
/// </summary>
public class InsuranceClaimPaidDomainEvent : DomainEvent
{
    public long ClaimId { get; }
    public long EmpSysId { get; }
    public decimal PaidAmount { get; }
    public DateTime PaidDate { get; }

    public InsuranceClaimPaidDomainEvent(long claimId, long empSysId, decimal paidAmount, DateTime paidDate)
    {
        ClaimId = claimId;
        EmpSysId = empSysId;
        PaidAmount = paidAmount;
        PaidDate = paidDate;
    }
}
