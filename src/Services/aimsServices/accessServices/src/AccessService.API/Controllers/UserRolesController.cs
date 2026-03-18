namespace AccessService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AccessService.Application.DTOs;
using AccessService.Application.CQRS.Commands;
using AccessService.Application.CQRS.Queries;

/// <summary>
/// REST API Controller for UserRole operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class UserRolesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserRolesController> _logger;

    public UserRolesController(IMediator mediator, ILogger<UserRolesController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get user role by ID
    /// </summary>
    [HttpGet("{roleId}")]
    public async Task<ActionResult<UserRoleDto>> GetUserRole(int roleId)
    {
        var query = new GetUserRoleByIdQuery { RoleId = roleId };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound($"User role not found with ID: {roleId}");

        return Ok(result);
    }

    /// <summary>
    /// Get all user roles for an employee
    /// </summary>
    [HttpGet("employee/{employeeSystemId}")]
    public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetUserRolesByEmployee(long employeeSystemId, [FromQuery] bool? activeOnly = false)
    {
        var query = new GetUserRolesByEmployeeIdQuery 
        { 
            EmployeeSystemId = employeeSystemId,
            ActiveOnly = activeOnly
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get all user roles by role type
    /// </summary>
    [HttpGet("type/{roleType}")]
    public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetUserRolesByType(char roleType)
    {
        var query = new GetUserRolesByTypeQuery { RoleType = roleType };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Assign a new role to user
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> AssignUserRole(CreateUserRoleDto request)
    {
        var command = new AssignUserRoleCommand
        {
            EmployeeSystemId = request.EmployeeSystemId,
            RoleType = request.RoleType,
            MenuAccess = request.MenuAccess,
            OrganizationId = request.OrganizationId,
            UnitId = request.UnitId,
            CalendarId = request.CalendarId
        };

        var roleId = await _mediator.Send(command);

        _logger.LogInformation($"Role assigned to employee ID: {request.EmployeeSystemId}, Role ID: {roleId}");

        return CreatedAtAction(nameof(GetUserRole), new { roleId }, roleId);
    }

    /// <summary>
    /// Update user role
    /// </summary>
    [HttpPut("{roleId}")]
    public async Task<ActionResult> UpdateUserRole(int roleId, UpdateUserRoleDto request)
    {
        try
        {
            var command = new UpdateUserRoleCommand
            {
                RoleId = roleId,
                MenuAccess = request.MenuAccess,
                OrganizationId = request.OrganizationId,
                UnitId = request.UnitId,
                CalendarId = request.CalendarId
            };

            await _mediator.Send(command);

            _logger.LogInformation($"Role updated: {roleId}");

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"User role not found with ID: {roleId}");
        }
    }

    /// <summary>
    /// Revoke user role
    /// </summary>
    [HttpDelete("{roleId}")]
    public async Task<ActionResult> RevokeUserRole(int roleId)
    {
        try
        {
            var command = new RevokeUserRoleCommand 
            { 
                RoleId = roleId,
                ClosureDate = DateTime.UtcNow
            };

            await _mediator.Send(command);

            _logger.LogInformation($"Role revoked: {roleId}");

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"User role not found with ID: {roleId}");
        }
    }
}
