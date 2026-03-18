using MediatR;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.CQRS.Commands;

/// <summary>
/// Command to enroll employee in insurance plan
/// </summary>
public class EnrollInsuranceCommand : IRequest<ApiResponse<InsuranceEnrollmentDto>>
{
    public long EmpSysId { get; set; }
    public long InsurancePlanId { get; set; }
    public string CoverageType { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public long CreatedBy { get; set; }
}

/// <summary>
/// Command to submit insurance claim
/// </summary>
public class SubmitClaimCommand : IRequest<ApiResponse<InsuranceClaimDto>>
{
    public long EmpSysId { get; set; }
    public long EnrollmentId { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public decimal ClaimAmount { get; set; }
    public DateTime ServiceDate { get; set; }
    public string HospitalName { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public long CreatedBy { get; set; }
}

/// <summary>
/// Command to approve insurance claim
/// </summary>
public class ApproveClaimCommand : IRequest<ApiResponse<InsuranceClaimDto>>
{
    public long ClaimId { get; set; }
    public decimal ApprovedAmount { get; set; }
    public long ApprovedBy { get; set; }
}

/// <summary>
/// Command to reject insurance claim
/// </summary>
public class RejectClaimCommand : IRequest<ApiResponse<InsuranceClaimDto>>
{
    public long ClaimId { get; set; }
    public string RejectionReason { get; set; } = string.Empty;
    public long RejectedBy { get; set; }
}

/// <summary>
/// Command to mark claim as paid
/// </summary>
public class MarkClaimAsPaidCommand : IRequest<ApiResponse<InsuranceClaimDto>>
{
    public long ClaimId { get; set; }
    public long PaidBy { get; set; }
}

/// <summary>
/// Command to terminate enrollment
/// </summary>
public class TerminateEnrollmentCommand : IRequest<ApiResponse<bool>>
{
    public long EnrollmentId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public long ModifiedBy { get; set; }
}

/// <summary>
/// Command to suspend enrollment
/// </summary>
public class SuspendEnrollmentCommand : IRequest<ApiResponse<bool>>
{
    public long EnrollmentId { get; set; }
    public long ModifiedBy { get; set; }
}

/// <summary>
/// Command to reactivate enrollment
/// </summary>
public class ReactivateEnrollmentCommand : IRequest<ApiResponse<bool>>
{
    public long EnrollmentId { get; set; }
    public long ModifiedBy { get; set; }
}
