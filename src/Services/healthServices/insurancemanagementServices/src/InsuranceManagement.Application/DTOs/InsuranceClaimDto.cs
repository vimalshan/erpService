namespace InsuranceManagement.Application.DTOs;

/// <summary>
/// DTO for Insurance Claim
/// </summary>
public class InsuranceClaimDto
{
    public long ClaimId { get; set; }
    public long EmpSysId { get; set; }
    public long EnrollmentId { get; set; }
    public long InsurancePlanId { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public decimal ClaimAmount { get; set; }
    public decimal ReimbursableAmount { get; set; }
    public decimal ApprovedAmount { get; set; }
    public DateTime ServiceDate { get; set; }
    public string HospitalName { get; set; } = string.Empty;
    public string ClaimRemarks { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTime? PaidDate { get; set; }
}

/// <summary>
/// DTO for submitting Insurance Claim
/// </summary>
public class SubmitInsuranceClaimDto
{
    public long EnrollmentId { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public decimal ClaimAmount { get; set; }
    public DateTime ServiceDate { get; set; }
    public string HospitalName { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
}

/// <summary>
/// DTO for approving Insurance Claim
/// </summary>
public class ApproveInsuranceClaimDto
{
    public long ClaimId { get; set; }
    public decimal ApprovedAmount { get; set; }
}

/// <summary>
/// DTO for rejecting Insurance Claim
/// </summary>
public class RejectInsuranceClaimDto
{
    public long ClaimId { get; set; }
    public string RejectionReason { get; set; } = string.Empty;
}
