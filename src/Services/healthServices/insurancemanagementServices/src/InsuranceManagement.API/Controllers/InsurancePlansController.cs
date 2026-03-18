using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InsuranceManagement.Application.CQRS.Commands;
using InsuranceManagement.Application.CQRS.Queries;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.API.Controllers;

/// <summary>
/// API Controller for Insurance Plan Management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class InsurancePlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsurancePlansController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Get insurance plan by ID
    /// </summary>
    [HttpGet("{id}", Name = nameof(GetInsurancePlanById))]
    [ProducesResponseType(typeof(ApiResponse<InsurancePlanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<InsurancePlanDto>>> GetInsurancePlanById(long id)
    {
        var query = new GetInsurancePlanByIdQuery { PlanId = id };
        var result = await _mediator.Send(query);
        
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Get all active insurance plans
    /// </summary>
    [HttpGet("active", Name = nameof(GetActiveInsurancePlans))]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<List<InsurancePlanDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<InsurancePlanDto>>>> GetActiveInsurancePlans()
    {
        var query = new GetAllActiveInsurancePlansQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get all insurance plans (paginated)
    /// </summary>
    [HttpGet(Name = nameof(GetAllInsurancePlans))]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<InsurancePlanDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<InsurancePlanDto>>>> GetAllInsurancePlans(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetAllInsurancePlansQuery 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize 
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create new insurance plan
    /// </summary>
    [HttpPost(Name = nameof(CreateInsurancePlan))]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<InsurancePlanDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<InsurancePlanDto>>> CreateInsurancePlan(
        [FromBody] CreateInsurancePlanDto dto)
    {
        var command = new CreateInsurancePlanCommand
        {
            PlanName = dto.PlanName,
            PlanDescription = dto.PlanDescription,
            PremiumRate = dto.PremiumRate,
            MinPremium = dto.MinPremium,
            MaxPremium = dto.MaxPremium,
            CoverageDetails = dto.CoverageDetails,
            CreatedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtRoute(nameof(GetInsurancePlanById), 
            new { id = result.Data?.InsurancePlanId }, result);
    }

    /// <summary>
    /// Update insurance plan
    /// </summary>
    [HttpPut("{id}", Name = nameof(UpdateInsurancePlan))]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<InsurancePlanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<InsurancePlanDto>>> UpdateInsurancePlan(
        long id,
        [FromBody] UpdateInsurancePlanDto dto)
    {
        var command = new UpdateInsurancePlanCommand
        {
            PlanId = id,
            PlanName = dto.PlanName,
            PlanDescription = dto.PlanDescription,
            PremiumRate = dto.PremiumRate,
            MinPremium = dto.MinPremium,
            MaxPremium = dto.MaxPremium,
            CoverageDetails = dto.CoverageDetails,
            ModifiedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Deactivate insurance plan
    /// </summary>
    [HttpPatch("{id}/deactivate", Name = nameof(DeactivateInsurancePlan))]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> DeactivateInsurancePlan(long id)
    {
        var command = new DeactivateInsurancePlanCommand
        {
            PlanId = id,
            ModifiedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Activate insurance plan
    /// </summary>
    [HttpPatch("{id}/activate", Name = nameof(ActivateInsurancePlan))]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> ActivateInsurancePlan(long id)
    {
        var command = new ActivateInsurancePlanCommand
        {
            PlanId = id,
            ModifiedBy = GetUserId()
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    private long GetUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("user_id");
        return long.TryParse(userIdClaim?.Value, out var userId) ? userId : 0;
    }
}
