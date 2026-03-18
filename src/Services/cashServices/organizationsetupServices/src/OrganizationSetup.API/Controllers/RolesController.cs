using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.Roles.Commands;
using OrganizationSetup.Application.Roles.Queries;

namespace OrganizationSetup.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRolesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{roleId}")]
    public async Task<ActionResult<RoleDto>> GetRoleById(long roleId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRoleByIdQuery(roleId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetRoleById), new { result.RoleId }, result);
    }
}
