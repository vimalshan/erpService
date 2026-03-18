namespace AccessService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AccessService.Application.DTOs;
using AccessService.Application.CQRS.Commands;
using AccessService.Application.CQRS.Queries;

/// <summary>
/// REST API Controller for UserMap operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class UserMapsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserMapsController> _logger;

    public UserMapsController(IMediator mediator, ILogger<UserMapsController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get user map by employee system ID
    /// </summary>
    [HttpGet("{employeeSystemId}")]
    public async Task<ActionResult<UserMapDto>> GetUserMap(long employeeSystemId)
    {
        var query = new GetUserMapByEmployeeIdQuery { EmployeeSystemId = employeeSystemId };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound($"User map not found for employee ID: {employeeSystemId}");

        return Ok(result);
    }

    /// <summary>
    /// Get all user maps
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserMapDto>>> GetAllUserMaps([FromQuery] bool? activeOnly = false)
    {
        var query = new GetAllUserMapsQuery { ActiveOnly = activeOnly };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create a new user map
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> CreateUserMap(CreateUserMapDto request)
    {
        var command = new CreateUserMapCommand { EmployeeSystemId = request.EmployeeSystemId };
        var result = await _mediator.Send(command);

        _logger.LogInformation($"UserMap created for employee ID: {request.EmployeeSystemId}");

        return CreatedAtAction(nameof(GetUserMap), new { employeeSystemId = request.EmployeeSystemId }, result);
    }

    /// <summary>
    /// Activate user map
    /// </summary>
    [HttpPut("{employeeSystemId}/activate")]
    public async Task<ActionResult> ActivateUserMap(long employeeSystemId, [FromBody] DateTime effectiveDate)
    {
        var command = new ActivateUserMapCommand 
        { 
            EmployeeSystemId = employeeSystemId,
            EffectiveDate = effectiveDate
        };

        await _mediator.Send(command);

        _logger.LogInformation($"UserMap activated for employee ID: {employeeSystemId}");

        return NoContent();
    }

    /// <summary>
    /// Update user map effective/closure dates
    /// </summary>
    [HttpPut("{employeeSystemId}")]
    public async Task<ActionResult> UpdateUserMap(long employeeSystemId, [FromBody] UpdateUserMapRequest request)
    {
        try
        {
            if (request.EffectiveDate.HasValue)
            {
                var activateCmd = new ActivateUserMapCommand 
                { 
                    EmployeeSystemId = employeeSystemId,
                    EffectiveDate = request.EffectiveDate.Value
                };
                await _mediator.Send(activateCmd);
            }

            if (request.ClosureDate.HasValue)
            {
                var deactivateCmd = new DeactivateUserMapCommand 
                { 
                    EmployeeSystemId = employeeSystemId,
                    ClosureDate = request.ClosureDate.Value
                };
                await _mediator.Send(deactivateCmd);
            }

            _logger.LogInformation($"UserMap updated for employee ID: {employeeSystemId}");

            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"User map not found for employee ID: {employeeSystemId}");
        }
    }

    /// <summary>
    /// Deactivate/delete user map
    /// </summary>
    [HttpDelete("{employeeSystemId}")]
    public async Task<ActionResult> DeleteUserMap(long employeeSystemId)
    {
        try
        {
            var command = new DeactivateUserMapCommand 
            { 
                EmployeeSystemId = employeeSystemId,
                ClosureDate = DateTime.UtcNow.AddDays(1)
            };

            await _mediator.Send(command);

            _logger.LogInformation($"UserMap deactivated for employee ID: {employeeSystemId}");

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"User map not found for employee ID: {employeeSystemId}");
        }
    }
}

/// <summary>Request model for UserMap update</summary>
public class UpdateUserMapRequest
{
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ClosureDate { get; set; }
    public long? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
