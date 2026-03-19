using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityService.Application.Commands.Users;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;

namespace SecurityService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public sealed class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    public RolesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all roles.</summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll(CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllRolesQuery(), ct));

    /// <summary>Get role by ID.</summary>
    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<ActionResult<RoleDto>> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetRoleByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new role.</summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.RoleId }, result);
    }

    /// <summary>Update a role.</summary>
    [HttpPut("{id:long}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<RoleDto>> Update(long id, [FromBody] UpdateRoleCommand command, CancellationToken ct = default)
    {
        if (id != command.RoleId) return BadRequest("ID mismatch.");
        return Ok(await _mediator.Send(command, ct));
    }

    /// <summary>Get menus assigned to a role.</summary>
    [HttpGet("{id:long}/menus")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MenuDto>>> GetMenus(long id, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetMenusByRoleQuery(id), ct));

    /// <summary>Assign a menu to a role.</summary>
    [HttpPost("{id:long}/menus")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AssignMenu(
        long id, [FromBody] AssignMenuRequest request, CancellationToken ct = default)
    {
        await _mediator.Send(
            new AssignMenuToRoleCommand(id, request.MenuId, request.AssignedBy, request.AssignedByNum), ct);
        return NoContent();
    }

    /// <summary>Unassign a menu from a role.</summary>
    [HttpDelete("{id:long}/menus/{menuId:long}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UnassignMenu(long id, long menuId, CancellationToken ct = default)
    {
        await _mediator.Send(new UnassignMenuFromRoleCommand(id, menuId), ct);
        return NoContent();
    }
}

public record AssignMenuRequest(long MenuId, string AssignedBy, long AssignedByNum);
