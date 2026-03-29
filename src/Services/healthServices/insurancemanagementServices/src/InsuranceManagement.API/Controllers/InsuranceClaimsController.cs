using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InsuranceManagement.Application.CQRS.Commands;
using InsuranceManagement.Application.CQRS.Queries;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.API.Controllers;

/// <summary>Request body for calculate-reimbursement endpoint</summary>
public record CalculateReimbursementRequest(
    decimal ClaimAmount,
    string ClaimType,
    decimal CopayPercentage = 20.0m);

/// <summary>
/// API Controller for Insurance Claims Management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class InsuranceClaimsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsuranceClaimsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Get claim by ID
    /// </summary>
    [HttpGet("{id}", Name = nameof(GetClaimById))]
    [ProducesResponseType(typeof(ApiResponse<InsuranceClaimDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<InsuranceClaimDto>>> GetClaimById(long id)
    {
        var query = new GetInsuranceClaimByIdQuery { ClaimId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get employee's claims (paginated)
    /// </summary>
    [HttpGet("employee/{empId}", Name = nameof(GetEmployeeClaims))]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<InsuranceClaimDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<InsuranceClaimDto>>>> GetEmployeeClaims(
        long empId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null)
    {
        var query = new GetEmployeeClaimsQuery 
        { 
            EmpSysId = empId, 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            Status = status
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get enrollment claims
    /// </summary>
    [HttpGet("enrollment/{enrollmentId}", Name = nameof(GetEnrollmentClaims))]
    [ProducesResponseType(typeof(ApiResponse<List<InsuranceClaimDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<InsuranceClaimDto>>>> GetEnrollmentClaims(long enrollmentId)
    {
        var query = new GetEnrollmentClaimsQuery { EnrollmentId = enrollmentId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Submit insurance claim
    /// </summary>
    [HttpPost(Name = nameof(SubmitClaim))]
    [ProducesResponseType(typeof(ApiResponse<InsuranceClaimDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<InsuranceClaimDto>>> SubmitClaim(
        [FromBody] SubmitInsuranceClaimDto dto)
    {
        var command = new SubmitClaimCommand
        {
            EmpSysId = GetEmpSysId(),
            EnrollmentId = dto.EnrollmentId,
            ClaimType = dto.ClaimType,
            ClaimAmount = dto.ClaimAmount,
            ServiceDate = dto.ServiceDate,
            HospitalName = dto.HospitalName,
            Remarks = dto.Remarks,
            CreatedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtRoute(nameof(GetClaimById), 
            new { id = result.Data?.ClaimId }, result);
    }

    /// <summary>
    /// Get claims pending approval
    /// </summary>
    [HttpGet("pending/approval", Name = nameof(GetClaimsForApproval))]
    [Authorize(Roles = "Admin,InsuranceManager")]
    [ProducesResponseType(typeof(ApiResponse<List<InsuranceClaimDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<InsuranceClaimDto>>>> GetClaimsForApproval(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetClaimsForApprovalQuery 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Approve insurance claim
    /// </summary>
    [HttpPatch("{id}/approve", Name = nameof(ApproveClaim))]
    [Authorize(Roles = "Admin,InsuranceManager")]
    [ProducesResponseType(typeof(ApiResponse<InsuranceClaimDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<InsuranceClaimDto>>> ApproveClaim(
        long id,
        [FromBody] ApproveInsuranceClaimDto dto)
    {
        var command = new ApproveClaimCommand
        {
            ClaimId = id,
            ApprovedAmount = dto.ApprovedAmount,
            ApprovedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Reject insurance claim
    /// </summary>
    [HttpPatch("{id}/reject", Name = nameof(RejectClaim))]
    [Authorize(Roles = "Admin,InsuranceManager")]
    [ProducesResponseType(typeof(ApiResponse<InsuranceClaimDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<InsuranceClaimDto>>> RejectClaim(
        long id,
        [FromBody] RejectInsuranceClaimDto dto)
    {
        var command = new RejectClaimCommand
        {
            ClaimId = id,
            RejectionReason = dto.RejectionReason,
            RejectedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Mark claim as paid
    /// </summary>
    [HttpPatch("{id}/mark-paid", Name = nameof(MarkClaimAsPaid))]
    [Authorize(Roles = "Admin,InsuranceManager")]
    [ProducesResponseType(typeof(ApiResponse<InsuranceClaimDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<InsuranceClaimDto>>> MarkClaimAsPaid(long id)
    {
        var command = new MarkClaimAsPaidCommand
        {
            ClaimId = id,
            PaidBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Calculate claim reimbursement
    /// </summary>
    [HttpPost("calculate-reimbursement", Name = nameof(CalculateReimbursement))]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<decimal>>> CalculateReimbursement(
        [FromBody] CalculateReimbursementRequest dto)
    {
        var query = new CalculateClaimReimbursementQuery
        {
            ClaimAmount = dto.ClaimAmount,
            ClaimType = dto.ClaimType,
            CopayPercentage = dto.CopayPercentage
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    private long GetUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("user_id");
        return long.TryParse(userIdClaim?.Value, out var userId) ? userId : 0;
    }

    private long GetEmpSysId()
    {
        var empIdClaim = User.FindFirst("emp_sys_id") ?? User.FindFirst("employee_id");
        return long.TryParse(empIdClaim?.Value, out var empId) ? empId : 0;
    }
}
