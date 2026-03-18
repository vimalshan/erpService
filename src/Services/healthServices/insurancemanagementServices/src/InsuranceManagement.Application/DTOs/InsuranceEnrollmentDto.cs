namespace InsuranceManagement.Application.DTOs;

/// <summary>
/// DTO for Insurance Enrollment
/// </summary>
public class InsuranceEnrollmentDto
{
    public long EnrollmentId { get; set; }
    public long EmpSysId { get; set; }
    public long InsurancePlanId { get; set; }
    public string CoverageType { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal MonthlyPremium { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? TerminationDate { get; set; }
    public string? TerminationReason { get; set; }
    public InsurancePlanDto? Plan { get; set; }
}

/// <summary>
/// DTO for creating Insurance Enrollment
/// </summary>
public class CreateInsuranceEnrollmentDto
{
    public long EmpSysId { get; set; }
    public long InsurancePlanId { get; set; }
    public string CoverageType { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public DateTime EffectiveDate { get; set; }
}

/// <summary>
/// DTO for Insurance Enrollment Details
/// </summary>
public class InsuranceEnrollmentDetailDto : InsuranceEnrollmentDto
{
    public List<InsuranceClaimDto> Claims { get; set; } = new();
}
