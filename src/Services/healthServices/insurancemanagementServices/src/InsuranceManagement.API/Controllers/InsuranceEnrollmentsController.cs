using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InsuranceManagement.Application.CQRS.Commands;
using InsuranceManagement.Application.CQRS.Queries;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.API.Controllers;

/// <summary>
/// API Controller for Insurance Enrollment Management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class InsuranceEnrollmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsuranceEnrollmentsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Get enrollment by ID
    /// </summary>
    [HttpGet("{id}", Name = nameof(GetEnrollmentById))]
    [ProducesResponseType(typeof(ApiResponse<InsuranceEnrollmentDetailDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<InsuranceEnrollmentDetailDto>>> GetEnrollmentById(long id)
    {
        var query = new GetInsuranceEnrollmentByIdQuery { EnrollmentId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get employee's active enrollments
    /// </summary>
    [HttpGet("employee/{empId}/active", Name = nameof(GetEmployeeActiveEnrollments))]
    [ProducesResponseType(typeof(ApiResponse<List<InsuranceEnrollmentDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<InsuranceEnrollmentDto>>>> GetEmployeeActiveEnrollments(long empId)
    {
        var query = new GetEmployeeActiveEnrollmentsQuery { EmpSysId = empId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get employee's all enrollments
    /// </summary>
    [HttpGet("employee/{empId}", Name = nameof(GetEmployeeAllEnrollments))]
    [ProducesResponseType(typeof(ApiResponse<List<InsuranceEnrollmentDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<InsuranceEnrollmentDto>>>> GetEmployeeAllEnrollments(long empId)
    {
        var query = new GetEmployeeAllEnrollmentsQuery { EmpSysId = empId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Enroll employee in insurance plan
    /// </summary>
    [HttpPost(Name = nameof(EnrollEmployee))]
    [Authorize(Roles = "Admin,HRManager")]
    [ProducesResponseType(typeof(ApiResponse<InsuranceEnrollmentDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<InsuranceEnrollmentDto>>> EnrollEmployee(
        [FromBody] CreateInsuranceEnrollmentDto dto)
    {
        var command = new EnrollInsuranceCommand
        {
            EmpSysId = dto.EmpSysId,
            InsurancePlanId = dto.InsurancePlanId,
            CoverageType = dto.CoverageType,
            EnrollmentDate = dto.EnrollmentDate,
            EffectiveDate = dto.EffectiveDate,
            CreatedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtRoute(nameof(GetEnrollmentById), 
            new { id = result.Data?.EnrollmentId }, result);
    }

    /// <summary>
    /// Terminate enrollment
    /// </summary>
    [HttpPatch("{id}/terminate", Name = nameof(TerminateEnrollment))]
    [Authorize(Roles = "Admin,HRManager")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> TerminateEnrollment(
        long id,
        [FromBody] dynamic dto)
    {
        var command = new TerminateEnrollmentCommand
        {
            EnrollmentId = id,
            Reason = dto.reason ?? "N/A",
            ModifiedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Suspend enrollment
    /// </summary>
    [HttpPatch("{id}/suspend", Name = nameof(SuspendEnrollment))]
    [Authorize(Roles = "Admin,HRManager")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> SuspendEnrollment(long id)
    {
        var command = new SuspendEnrollmentCommand
        {
            EnrollmentId = id,
            ModifiedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Reactivate enrollment
    /// </summary>
    [HttpPatch("{id}/reactivate", Name = nameof(ReactivateEnrollment))]
    [Authorize(Roles = "Admin,HRManager")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> ReactivateEnrollment(long id)
    {
        var command = new ReactivateEnrollmentCommand
        {
            EnrollmentId = id,
            ModifiedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Check employee eligibility
    /// </summary>
    [HttpGet("employee/{empId}/eligibility", Name = nameof(CheckEligibility))]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<string>>> CheckEligibility(long empId)
    {
        var query = new CheckEmployeeEligibilityQuery { EmpSysId = empId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    private long GetUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("user_id");
        return long.TryParse(userIdClaim?.Value, out var userId) ? userId : 0;
    }
}
