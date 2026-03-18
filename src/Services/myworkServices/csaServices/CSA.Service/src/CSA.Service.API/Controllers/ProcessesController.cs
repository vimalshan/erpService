using CSA.Service.Application.Commands.Processes;
using CSA.Service.Application.DTOs;
using CSA.Service.Application.Queries.Processes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSA.Service.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProcessDto>>> GetAll(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllProcessesQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProcessDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProcessByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:long}/subprocesses")]
    public async Task<ActionResult<IEnumerable<SubProcessDto>>> GetSubProcesses(long id, CancellationToken ct) =>
        Ok(await mediator.Send(new GetSubProcessesByProcessQuery(id), ct));

    [HttpPost]
    public async Task<ActionResult<ProcessDto>> Create([FromBody] CreateProcessDto dto, CancellationToken ct)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new CreateProcessCommand(dto, userId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ProcessId }, result);
    }

    [HttpPost("subprocesses")]
    public async Task<ActionResult<SubProcessDto>> CreateSubProcess([FromBody] CreateSubProcessDto dto, CancellationToken ct)
    {
        var userId = GetUserId();
        return Ok(await mediator.Send(new CreateSubProcessCommand(dto, userId), ct));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await mediator.Send(new DeleteProcessCommand(id), ct);
        return NoContent();
    }

    private long GetUserId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim != null && long.TryParse(claim.Value, out var id) ? id : 0;
    }
}
