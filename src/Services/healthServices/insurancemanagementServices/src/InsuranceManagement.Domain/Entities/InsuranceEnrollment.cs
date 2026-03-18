using InsuranceManagement.Domain.ValueObjects;
using InsuranceManagement.Domain.Common;
namespace InsuranceManagement.Domain.Entities;

/// <summary>
/// Insurance Enrollment entity representing employee enrollment in insurance plans
/// </summary>
public class InsuranceEnrollment : AggregateRoot
{
    public long EnrollmentId { get; private set; }
    public long EmpSysId { get; private set; }
    public long InsurancePlanId { get; private set; }
    public CoverageType CoverageType { get; private set; } = CoverageType.Employee_Coverage;
    public DateTime EnrollmentDate { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public decimal MonthlyPremium { get; private set; }
    public EnrollmentStatus Status { get; private set; } = EnrollmentStatus.Active_Status;
    public DateTime? TerminationDate { get; private set; }
    public string? TerminationReason { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public long? ModifiedBy { get; private set; }

    // Navigation properties
    public InsurancePlan? InsurancePlan { get; set; }
    public List<InsuranceClaim> Claims { get; private set; } = new();

    // EF constructor
    private InsuranceEnrollment() { }

    public InsuranceEnrollment(
        long empSysId,
        long insurancePlanId,
        CoverageType coverageType,
        DateTime enrollmentDate,
        DateTime effectiveDate,
        decimal monthlyPremium,
        long createdBy)
    {
        if (empSysId <= 0)
            throw new ArgumentException("Employee ID must be greater than zero", nameof(empSysId));

        if (insurancePlanId <= 0)
            throw new ArgumentException("Insurance Plan ID must be greater than zero", nameof(insurancePlanId));

        if (monthlyPremium < 0)
            throw new ArgumentException("Monthly premium cannot be negative", nameof(monthlyPremium));

        if (enrollmentDate > effectiveDate)
            throw new ArgumentException("Enrollment date cannot be after effective date", nameof(enrollmentDate));

        EmpSysId = empSysId;
        InsurancePlanId = insurancePlanId;
        CoverageType = coverageType ?? throw new ArgumentNullException(nameof(coverageType));
        EnrollmentDate = enrollmentDate;
        EffectiveDate = effectiveDate;
        MonthlyPremium = monthlyPremium;
        Status = EnrollmentStatus.Active_Status;
        CreatedOn = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void UpdateMonthlyPremium(decimal newPremium, long modifiedBy)
    {
        if (newPremium < 0)
            throw new ArgumentException("Monthly premium cannot be negative", nameof(newPremium));

        MonthlyPremium = newPremium;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    public void Terminate(string reason, long modifiedBy)
    {
        if (!Status.IsActive)
            throw new InvalidOperationException("Cannot terminate an inactive enrollment");

        Status = EnrollmentStatus.Terminated_Status;
        TerminationDate = DateTime.UtcNow;
        TerminationReason = reason;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = modifiedBy;

        // Raise domain event
        AddDomainEvent(new InsuranceEnrollmentTerminatedDomainEvent(
            EnrollmentId, EmpSysId, InsurancePlanId, reason));
    }

    public void Suspend(long modifiedBy)
    {
        if (!Status.IsActive)
            throw new InvalidOperationException("Can only suspend active enrollments");

        Status = EnrollmentStatus.Suspended_Status;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    public void Reactivate(long modifiedBy)
    {
        if (!Status.IsSuspended)
            throw new InvalidOperationException("Can only reactivate suspended enrollments");

        Status = EnrollmentStatus.Active_Status;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }
}

/// <summary>
/// Domain event raised when enrollment is terminated
/// </summary>
public class InsuranceEnrollmentTerminatedDomainEvent : DomainEvent
{
    public long EnrollmentId { get; }
    public long EmpSysId { get; }
    public long InsurancePlanId { get; }
    public string TerminationReason { get; }

    public InsuranceEnrollmentTerminatedDomainEvent(long enrollmentId, long empSysId, long insurancePlanId, string reason)
    {
        EnrollmentId = enrollmentId;
        EmpSysId = empSysId;
        InsurancePlanId = insurancePlanId;
        TerminationReason = reason;
    }
}
