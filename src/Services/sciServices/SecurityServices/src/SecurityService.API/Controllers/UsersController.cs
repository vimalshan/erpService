using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SecurityService.Application.Commands.Users;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;

namespace SecurityService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all users.</summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserListDto>>> GetAll(
        [FromQuery] bool activeOnly = false, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllUsersQuery(activeOnly), ct));

    /// <summary>Get a user by ID.</summary>
    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get a user by code.</summary>
    [HttpGet("code/{code}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetByCode(string code, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetUserByCodeQuery(code), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new user.</summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.UserId }, result);
    }

    /// <summary>Update a user.</summary>
    [HttpPut("{id:long}")]
    [Authorize(Policy = "SecurityManager")]
    public async Task<ActionResult<UserDto>> Update(long id, [FromBody] UpdateUserCommand command, CancellationToken ct = default)
    {
        if (id != command.UserId) return BadRequest("ID mismatch.");
        return Ok(await _mediator.Send(command, ct));
    }

    /// <summary>Deactivate a user.</summary>
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Deactivate(long id, CancellationToken ct = default)
    {
        await _mediator.Send(new DeactivateUserCommand(id, DateTime.UtcNow), ct);
        return NoContent();
    }

    /// <summary>Get roles assigned to a user.</summary>
    [HttpGet("{id:long}/roles")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetRoles(long id, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetUserRolesQuery(id), ct));

    /// <summary>Assign a role to a user.</summary>
    [HttpPost("{id:long}/roles")]
    [Authorize(Policy = "SecurityManager")]
    public async Task<IActionResult> AssignRole(long id, [FromBody] AssignRoleRequest request, CancellationToken ct = default)
    {
        await _mediator.Send(new AssignRoleCommand(id, request.RoleId, request.StartDate, request.EndDate, request.AssignedBy), ct);
        return NoContent();
    }

    /// <summary>Revoke a role from a user.</summary>
    [HttpDelete("{id:long}/roles/{roleId:long}")]
    [Authorize(Policy = "SecurityManager")]
    public async Task<IActionResult> RevokeRole(long id, long roleId, CancellationToken ct = default)
    {
        await _mediator.Send(new RevokeRoleCommand(id, roleId), ct);
        return NoContent();
    }
}

public record AssignRoleRequest(long RoleId, DateTime StartDate, DateTime? EndDate, string AssignedBy);
