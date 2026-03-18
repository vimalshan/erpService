using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Application.CQRS.Queries;
using MediatR;

namespace InsuranceManagement.API.GraphQL;

/// <summary>
/// GraphQL Query type for Insurance Management
/// </summary>
public class InsuranceQuery
{
    private readonly IMediator _mediator;

    public InsuranceQuery(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Insurance Plans
    public async Task<ApiResponse<InsurancePlanDto>> GetInsurancePlanAsync(long id)
    {
        var query = new GetInsurancePlanByIdQuery { PlanId = id };
        return await _mediator.Send(query);
    }

    public async Task<ApiResponse<List<InsurancePlanDto>>> GetAllActiveInsurancePlansAsync()
    {
        var query = new GetAllActiveInsurancePlansQuery();
        return await _mediator.Send(query);
    }

    public async Task<ApiResponse<PaginatedResponse<InsurancePlanDto>>> GetAllInsurancePlansAsync(int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetAllInsurancePlansQuery { PageNumber = pageNumber, PageSize = pageSize };
        return await _mediator.Send(query);
    }

    // Insurance Enrollments
    public async Task<ApiResponse<InsuranceEnrollmentDetailDto>> GetInsuranceEnrollmentAsync(long id)
    {
        var query = new GetInsuranceEnrollmentByIdQuery { EnrollmentId = id };
        return await _mediator.Send(query);
    }

    public async Task<ApiResponse<List<InsuranceEnrollmentDto>>> GetEmployeeActiveEnrollmentsAsync(long empId)
    {
        var query = new GetEmployeeActiveEnrollmentsQuery { EmpSysId = empId };
        return await _mediator.Send(query);
    }

    public async Task<ApiResponse<List<InsuranceEnrollmentDto>>> GetEmployeeAllEnrollmentsAsync(long empId)
    {
        var query = new GetEmployeeAllEnrollmentsQuery { EmpSysId = empId };
        return await _mediator.Send(query);
    }

    // Insurance Claims
    public async Task<ApiResponse<InsuranceClaimDto>> GetInsuranceClaimAsync(long id)
    {
        var query = new GetInsuranceClaimByIdQuery { ClaimId = id };
        return await _mediator.Send(query);
    }

    public async Task<ApiResponse<PaginatedResponse<InsuranceClaimDto>>> GetEmployeeClaimsAsync(
        long empId, int pageNumber = 1, int pageSize = 10, string? status = null)
    {
        var query = new GetEmployeeClaimsQuery 
        { 
            EmpSysId = empId, 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            Status = status
        };
        return await _mediator.Send(query);
    }

    public async Task<ApiResponse<List<InsuranceClaimDto>>> GetEnrollmentClaimsAsync(long enrollmentId)
    {
        var query = new GetEnrollmentClaimsQuery { EnrollmentId = enrollmentId };
        return await _mediator.Send(query);
    }

    public async Task<ApiResponse<List<InsuranceClaimDto>>> GetClaimsForApprovalAsync()
    {
        var query = new GetClaimsForApprovalQuery();
        return await _mediator.Send(query);
    }

    public async Task<ApiResponse<string>> CheckEmployeeEligibilityAsync(long empId)
    {
        var query = new CheckEmployeeEligibilityQuery { EmpSysId = empId };
        return await _mediator.Send(query);
    }

    public async Task<ApiResponse<decimal>> CalculateClaimReimbursementAsync(
        decimal claimAmount, string claimType, decimal copayPercentage = 20.0m)
    {
        var query = new CalculateClaimReimbursementQuery
        {
            ClaimAmount = claimAmount,
            ClaimType = claimType,
            CopayPercentage = copayPercentage
        };
        return await _mediator.Send(query);
    }
}
