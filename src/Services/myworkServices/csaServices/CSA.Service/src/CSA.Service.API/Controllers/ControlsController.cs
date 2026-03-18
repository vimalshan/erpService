using CSA.Service.Application.Commands.Controls;
using CSA.Service.Application.DTOs;
using CSA.Service.Application.Queries.Controls;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSA.Service.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ControlsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ControlDto>>> GetAll(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllControlsQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ControlDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetControlByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("process/{processId:long}")]
    public async Task<ActionResult<IEnumerable<ControlDto>>> GetByProcess(long processId, CancellationToken ct) =>
        Ok(await mediator.Send(new GetControlsByProcessQuery(processId), ct));

    [HttpGet("{id:long}/evidences")]
    public async Task<ActionResult<IEnumerable<EvidenceDto>>> GetEvidences(long id, CancellationToken ct) =>
        Ok(await mediator.Send(new GetEvidencesByControlQuery(id), ct));

    [HttpPost]
    public async Task<ActionResult<ControlDto>> Create([FromBody] CreateControlDto dto, CancellationToken ct)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new CreateControlCommand(dto, userId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ControlId }, result);
    }

    [HttpPut]
    public async Task<ActionResult<ControlDto>> Update([FromBody] UpdateControlDto dto, CancellationToken ct)
    {
        var userId = GetUserId();
        return Ok(await mediator.Send(new UpdateControlCommand(dto, userId), ct));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await mediator.Send(new DeleteControlCommand(id), ct);
        return NoContent();
    }

    private long GetUserId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim != null && long.TryParse(claim.Value, out var id) ? id : 0;
    }
}
