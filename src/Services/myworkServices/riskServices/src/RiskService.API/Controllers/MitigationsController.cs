using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskService.Application.Commands.Mitigation;
using RiskService.Application.DTOs;
using RiskService.Application.Queries.Mitigation;

namespace RiskService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MitigationsController(IMediator mediator) : ControllerBase
{
    [HttpGet("risk/{riskId:long}")]
    public async Task<ActionResult<IReadOnlyList<MitigationDto>>> GetByRiskId(long riskId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetMitigationsByRiskIdQuery(riskId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<long>> Create([FromBody] CreateMitigationCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(id);
    }

    [HttpPost("{mitigationId:long}/actions")]
    public async Task<ActionResult<long>> AddAction(long mitigationId, [FromBody] AddActionRequest request, CancellationToken ct)
    {
        var id = await mediator.Send(new AddMitigationActionCommand(mitigationId, request.DueDate, request.Comments, request.CreatedBy), ct);
        if (id == 0) return NotFound();
        return Ok(id);
    }
}

public record AddActionRequest(DateTime DueDate, string Comments, long CreatedBy);
