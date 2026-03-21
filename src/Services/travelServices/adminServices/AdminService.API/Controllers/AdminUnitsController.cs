using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminService.Application.Commands;
using AdminService.Application.Queries;
using AdminService.Application.DTOs;

namespace AdminService.API.Controllers;

/// <summary>
/// Admin unit REST API controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class AdminUnitsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdminUnitsController> _logger;

    public AdminUnitsController(IMediator mediator, ILogger<AdminUnitsController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all admin units
    /// </summary>
    /// <returns>List of admin units</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AdminUnitDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all admin units");
        var result = await _mediator.Send(new GetAllAdminUnitsQuery(), cancellationToken);
        return Ok(ApiResponse<IEnumerable<AdminUnitDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get admin unit by ID
    /// </summary>
    /// <param name="id">Admin unit ID</param>
    /// <returns>Admin unit details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AdminUnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting admin unit by ID: {Id}", id);
        var result = await _mediator.Send(new GetAdminUnitByIdQuery(id), cancellationToken);
        
        if (result == null)
            return NotFound(ApiResponse<AdminUnitDto>.ErrorResponse("Admin unit not found"));

        return Ok(ApiResponse<AdminUnitDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Get admin units by type
    /// </summary>
    /// <param name="adminType">Admin type (T=Travel, S=Stay, M=Meeting)</param>
    /// <returns>List of admin units of specified type</returns>
    [HttpGet("type/{adminType}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AdminUnitDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByType(string adminType, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting admin units by type: {AdminType}", adminType);
        var result = await _mediator.Send(new GetAdminUnitsByTypeQuery(adminType), cancellationToken);
        return Ok(ApiResponse<IEnumerable<AdminUnitDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create new admin unit
    /// </summary>
    /// <param name="request">Create admin unit request</param>
    /// <returns>Created admin unit</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AdminUnitDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateAdminUnitRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating admin unit with code: {AdminCode}", request.AdminCode);
        
        var command = new CreateAdminUnitCommand(
            request.AdminCode,
            request.Name,
            request.AdminType,
            request.UnitCode,
            request.CabUnit,
            request.ImageUrl,
            request.SortOrder
        );

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, 
            ApiResponse<AdminUnitDto>.SuccessResponse(result, "Admin unit created successfully"));
    }

    /// <summary>
    /// Update admin unit
    /// </summary>
    /// <param name="id">Admin unit ID</param>
    /// <param name="request">Update admin unit request</param>
    /// <returns>Updated admin unit</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AdminUnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, UpdateAdminUnitRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating admin unit with ID: {Id}", id);
        
        if (id != request.Id)
            return BadRequest(ApiResponse<AdminUnitDto>.ErrorResponse("ID mismatch"));

        var command = new UpdateAdminUnitCommand(
            request.Id,
            request.Name,
            request.AdminType,
            request.UnitCode,
            request.CabUnit,
            request.ImageUrl,
            request.SortOrder
        );

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(ApiResponse<AdminUnitDto>.SuccessResponse(result, "Admin unit updated successfully"));
    }

    /// <summary>
    /// Delete admin unit
    /// </summary>
    /// <param name="id">Admin unit ID</param>
    /// <returns>Success or failure message</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting admin unit with ID: {Id}", id);
        
        var result = await _mediator.Send(new DeleteAdminUnitCommand(id), cancellationToken);
        
        if (!result)
            return NotFound(ApiResponse<bool>.ErrorResponse("Admin unit not found"));

        return Ok(ApiResponse<bool>.SuccessResponse(result, "Admin unit deleted successfully"));
    }
}
