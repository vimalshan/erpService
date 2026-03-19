using MediatR;
using MenuAndSecurityService.Application.Commands.GrantMenuAccess;
using MenuAndSecurityService.Application.Commands.RevokeMenuAccess;
using MenuAndSecurityService.Application.DTOs;
using MenuAndSecurityService.Application.Queries.GetMenusByRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuAndSecurityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleMenuAccessController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleMenuAccessController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("role/{roleId:long}")]
    public async Task<ActionResult<IEnumerable<RoleMenuAccessDto>>> GetByRole(long roleId)
    {
        var result = await _mediator.Send(new GetMenusByRoleQuery(roleId));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoleMenuAccessDto>> Grant([FromBody] GrantMenuAccessCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByRole), new { roleId = result.MenuRoleId }, result);
    }

    [HttpDelete("{accessId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Revoke(long accessId)
    {
        var result = await _mediator.Send(new RevokeMenuAccessCommand(accessId));
        if (!result) return NotFound();
        return NoContent();
    }
}
