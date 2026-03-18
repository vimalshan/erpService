using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamServices.Application.Commands;
using TeamServices.Application.DTOs;
using TeamServices.Application.Queries;

namespace TeamServices.API.Controllers;

[ApiController]
[Route("api/teams/{teamId:long}/unitmaps")]
[Authorize]
public class TeamUnitMapsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeamUnitMapsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<TeamUnitMapDto>>> GetByTeamId(long teamId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamUnitMapsByTeamIdQuery(teamId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TeamUnitMapDto>> Add(long teamId, [FromBody] AddTeamUnitMapCommand command, CancellationToken cancellationToken)
    {
        if (teamId != command.TeamId)
            return BadRequest("Route teamId does not match body TeamId.");
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByTeamId), new { teamId }, result);
    }

    [HttpPut("{mapId:long}")]
    public async Task<ActionResult<TeamUnitMapDto>> Update(long teamId, long mapId, [FromBody] UpdateTeamUnitMapCommand command, CancellationToken cancellationToken)
    {
        if (mapId != command.MapId || teamId != command.TeamId)
            return BadRequest("Route parameters do not match body.");
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{mapId:long}")]
    public async Task<IActionResult> Delete(long mapId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTeamUnitMapCommand(mapId), cancellationToken);
        return NoContent();
    }
}
