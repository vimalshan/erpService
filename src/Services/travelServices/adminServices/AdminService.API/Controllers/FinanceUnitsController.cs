using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminService.Application.Commands;
using AdminService.Application.Queries;
using AdminService.Application.DTOs;

namespace AdminService.API.Controllers;

/// <summary>
/// Finance unit REST API controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class FinanceUnitsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<FinanceUnitsController> _logger;

    public FinanceUnitsController(IMediator mediator, ILogger<FinanceUnitsController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all finance units
    /// </summary>
    /// <returns>List of finance units</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FinanceUnitDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all finance units");
        var result = await _mediator.Send(new GetAllFinanceUnitsQuery(), cancellationToken);
        return Ok(ApiResponse<IEnumerable<FinanceUnitDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get finance unit by ID
    /// </summary>
    /// <param name="id">Finance unit ID</param>
    /// <returns>Finance unit details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<FinanceUnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting finance unit by ID: {Id}", id);
        var result = await _mediator.Send(new GetFinanceUnitByIdQuery(id), cancellationToken);
        
        if (result == null)
            return NotFound(ApiResponse<FinanceUnitDto>.ErrorResponse("Finance unit not found"));

        return Ok(ApiResponse<FinanceUnitDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Create new finance unit
    /// </summary>
    /// <param name="request">Create finance unit request</param>
    /// <returns>Created finance unit</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FinanceUnitDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateAdminUnitRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating finance unit");
        
        var command = new CreateFinanceUnitCommand(
            request.AdminCode,
            request.UnitCode,
            request.Name,
            null
        );

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, 
            ApiResponse<FinanceUnitDto>.SuccessResponse(result, "Finance unit created successfully"));
    }
}
