using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamServices.Application.Commands;
using TeamServices.Application.DTOs;
using TeamServices.Application.Queries;

namespace TeamServices.API.Controllers;

[ApiController]
[Route("api/teams/{teamId:long}/employees")]
[Authorize]
public class TeamEmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeamEmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<TeamEmployeeMapDto>>> GetByTeamId(long teamId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTeamEmployeesByTeamIdQuery(teamId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<TeamEmployeeMapDto>>> GetActive(long teamId, [FromQuery] DateTime? asOfDate, CancellationToken cancellationToken)
    {
        var date = asOfDate ?? DateTime.UtcNow;
        var result = await _mediator.Send(new GetActiveTeamEmployeesQuery(teamId, date), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TeamEmployeeMapDto>> Add(long teamId, [FromBody] AddTeamEmployeeCommand command, CancellationToken cancellationToken)
    {
        if (teamId != command.TeamId)
            return BadRequest("Route teamId does not match body TeamId.");
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByTeamId), new { teamId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<TeamEmployeeMapDto>> Update(long teamId, long id, [FromBody] UpdateTeamEmployeeCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id || teamId != command.TeamId)
            return BadRequest("Route parameters do not match body.");
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTeamEmployeeCommand(id), cancellationToken);
        return NoContent();
    }
}
