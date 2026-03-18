using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ObjectiveService.Application.Features.ControlPoints.Commands;
using ObjectiveService.Application.Features.ControlPoints.Queries;
using ObjectiveService.Application.DTOs;
using ObjectiveService.Application.Common;

namespace ObjectiveService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ControlPointsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ControlPointsController> _logger;

    public ControlPointsController(IMediator mediator, ILogger<ControlPointsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get control point by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ControlPointDto>> GetControlPointById(decimal id)
    {
        _logger.LogInformation("Getting control point with ID: {ControlPointId}", id);
        var query = new GetControlPointByIdQuery(id);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Get control points by employee
    /// </summary>
    [HttpGet("employee/{employeeSysId}/{ddYearId}")]
    public async Task<ActionResult<List<ControlPointDto>>> GetControlPointsByEmployee(
        decimal employeeSysId, 
        decimal ddYearId)
    {
        _logger.LogInformation("Getting control points for employee {EmployeeSysId} and year {DDYearId}", 
            employeeSysId, ddYearId);
        
        var query = new GetControlPointsByEmployeeQuery(employeeSysId, ddYearId);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get all control points (paginated)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ControlPointDto>>> GetAllControlPoints(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting all control points - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
        
        var query = new GetAllControlPointsQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Create a new control point
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CommandResult<decimal>>> CreateControlPoint(CreateControlPointDto dto)
    {
        _logger.LogInformation("Creating control point for employee {EmployeeSysId}", dto.EmployeeSysId);
        
        var command = new CreateControlPointCommand
        {
            EmployeeSysId = dto.EmployeeSysId,
            DDYearId = dto.DDYearId,
            Source = dto.Source,
            RefId = dto.RefId,
            SerialNumber = dto.SerialNumber,
            Description = dto.Description,
            Category = dto.Category,
            UnitOfMeasurement = dto.UnitOfMeasurement,
            UnitFrom = dto.UnitFrom,
            UnitTo = dto.UnitTo,
            VersionNumber = dto.VersionNumber,
            Weightage = dto.Weightage,
            AccountabilityId = dto.AccountabilityId
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetControlPointById), new { id = result.Data }, result);
    }

    /// <summary>
    /// Update a control point
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<CommandResult>> UpdateControlPoint(
        decimal id, 
        UpdateControlPointDto dto)
    {
        _logger.LogInformation("Updating control point {ControlPointId}", id);
        
        var command = new UpdateControlPointCommand
        {
            Id = id,
            Description = dto.Description,
            UnitFrom = dto.UnitFrom,
            UnitTo = dto.UnitTo,
            Weightage = dto.Weightage
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete a control point
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<CommandResult>> DeleteControlPoint(decimal id)
    {
        _logger.LogInformation("Deleting control point {ControlPointId}", id);
        
        var command = new DeleteControlPointCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
