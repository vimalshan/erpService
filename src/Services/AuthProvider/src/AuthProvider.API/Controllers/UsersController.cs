using AuthProvider.Application.Commands;
using AuthProvider.Application.DTOs;
using AuthProvider.Application.Queries;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthProvider.API.Controllers;

/// <summary>
/// User management controller.
/// Demonstrates: CQRS (via MediatR), Authorization Policies, ILogger, Routing, API Versioning.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>Get a paged list of all users.</summary>
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(PagedResult<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        _logger.LogInformation("GetAll users page={Page} pageSize={PageSize}", page, pageSize);
        var result = await _mediator.Send(new GetAllUsersQuery(page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Get a user by their ID.</summary>
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "UserOrAdmin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get a user by email address.</summary>
    [HttpGet("by-email")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail([FromQuery] string email, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUserByEmailQuery(email), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Update a user's profile.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateUserCommand(id, dto.FirstName, dto.LastName), ct);
        return Ok(result);
    }

    /// <summary>Deactivate (soft-delete) a user.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var success = await _mediator.Send(new DeleteUserCommand(id), ct);
        return success ? NoContent() : NotFound();
    }

    /// <summary>Assign a role to a user.</summary>
    [HttpPost("{id:guid}/roles")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignRole(Guid id, [FromBody] AssignRoleDto dto, CancellationToken ct)
    {
        await _mediator.Send(new AssignRoleCommand(id, dto.RoleName), ct);
        return NoContent();
    }

    /// <summary>Get all available roles.</summary>
    [HttpGet("/api/v{version:apiVersion}/roles")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoles(CancellationToken ct)
    {
        var roles = await _mediator.Send(new GetAllRolesQuery(), ct);
        return Ok(roles);
    }
}
