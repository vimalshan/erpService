using MasterService.Application.Features.Skills.Commands;
using MasterService.Application.Features.Skills.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SkillsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] char? skillType, CancellationToken ct)
        => Ok(await mediator.Send(new GetSkillsQuery(skillType), ct));

    [HttpGet("{skillCode:long}")]
    public async Task<IActionResult> GetById(long skillCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSkillByCodeQuery(skillCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSkillCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { skillCode = result.SkillCode }, result);
    }

    [HttpPut("{skillCode:long}")]
    public async Task<IActionResult> Update(long skillCode, [FromBody] UpdateSkillCommand command, CancellationToken ct)
    {
        if (skillCode != command.SkillCode) return BadRequest("Route code and body code mismatch.");
        return Ok(await mediator.Send(command, ct));
    }

    [HttpDelete("{skillCode:long}")]
    public async Task<IActionResult> Close(long skillCode, CancellationToken ct)
    {
        await mediator.Send(new CloseSkillCommand(skillCode), ct);
        return NoContent();
    }
}
