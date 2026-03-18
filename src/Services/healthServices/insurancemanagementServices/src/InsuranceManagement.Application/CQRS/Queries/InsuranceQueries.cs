using MediatR;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.CQRS.Queries;

/// <summary>
/// Query to get insurance plan by ID
/// </summary>
public class GetInsurancePlanByIdQuery : IRequest<ApiResponse<InsurancePlanDto>>
{
    public long PlanId { get; set; }
}

/// <summary>
/// Query to get all active insurance plans
/// </summary>
public class GetAllActiveInsurancePlansQuery : IRequest<ApiResponse<List<InsurancePlanDto>>>
{
}

/// <summary>
/// Query to get all insurance plans
/// </summary>
public class GetAllInsurancePlansQuery : IRequest<ApiResponse<PaginatedResponse<InsurancePlanDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Query to get employee enrollment by ID
/// </summary>
public class GetInsuranceEnrollmentByIdQuery : IRequest<ApiResponse<InsuranceEnrollmentDetailDto>>
{
    public long EnrollmentId { get; set; }
}

/// <summary>
/// Query to get employee's active enrollments
/// </summary>
public class GetEmployeeActiveEnrollmentsQuery : IRequest<ApiResponse<List<InsuranceEnrollmentDto>>>
{
    public long EmpSysId { get; set; }
}

/// <summary>
/// Query to get employee's all enrollments
/// </summary>
public class GetEmployeeAllEnrollmentsQuery : IRequest<ApiResponse<List<InsuranceEnrollmentDto>>>
{
    public long EmpSysId { get; set; }
}

/// <summary>
/// Query to get insurance claim by ID
/// </summary>
public class GetInsuranceClaimByIdQuery : IRequest<ApiResponse<InsuranceClaimDto>>
{
    public long ClaimId { get; set; }
}

/// <summary>
/// Query to get employee's claims
/// </summary>
public class GetEmployeeClaimsQuery : IRequest<ApiResponse<PaginatedResponse<InsuranceClaimDto>>>
{
    public long EmpSysId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; }
}

/// <summary>
/// Query to get enrollment claims
/// </summary>
public class GetEnrollmentClaimsQuery : IRequest<ApiResponse<List<InsuranceClaimDto>>>
{
    public long EnrollmentId { get; set; }
}

/// <summary>
/// Query to get claims for approval
/// </summary>
public class GetClaimsForApprovalQuery : IRequest<ApiResponse<List<InsuranceClaimDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Query to calculate claim reimbursement
/// </summary>
public class CalculateClaimReimbursementQuery : IRequest<ApiResponse<decimal>>
{
    public decimal ClaimAmount { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public decimal CopayPercentage { get; set; } = 20.0m;
}

/// <summary>
/// Query to check employee eligibility
/// </summary>
public class CheckEmployeeEligibilityQuery : IRequest<ApiResponse<string>>
{
    public long EmpSysId { get; set; }
}
