using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskService.Application.Commands.Risk;
using RiskService.Application.DTOs;
using RiskService.Application.Queries.Risk;

namespace RiskService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RisksController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RiskDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllRisksQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<RiskDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRiskByIdQuery(id), ct);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<long>> Create([FromBody] CreateRiskCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateRiskCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        var result = await mediator.Send(command, ct);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("{id:long}/submit")]
    public async Task<IActionResult> Submit(long id, [FromBody] long submittedBy, CancellationToken ct)
    {
        var result = await mediator.Send(new SubmitRiskCommand(id, submittedBy), ct);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("{id:long}/approve")]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveRiskRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new ApproveRiskCommand(id, request.ApprovedBy, request.Remarks), ct);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("{id:long}/cancel")]
    public async Task<IActionResult> Cancel(long id, [FromBody] CancelRiskRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CancelRiskCommand(id, request.CancelledBy, request.Reason), ct);
        if (!result) return NotFound();
        return NoContent();
    }
}

public record ApproveRiskRequest(long ApprovedBy, string Remarks);
public record CancelRiskRequest(long CancelledBy, string Reason);
