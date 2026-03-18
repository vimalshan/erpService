using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskService.Application.Commands.SelfAssessment;
using RiskService.Application.DTOs;
using RiskService.Application.Queries.SelfAssessment;

namespace RiskService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SelfAssessmentsController(IMediator mediator) : ControllerBase
{
    [HttpGet("pending")]
    public async Task<ActionResult<IReadOnlyList<SelfAssessmentDto>>> GetPending(CancellationToken ct)
    {
        var result = await mediator.Send(new GetPendingSelfAssessmentsQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<long>> Create([FromBody] CreateSelfAssessmentCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Ok(id);
    }

    [HttpPost("{id:long}/complete")]
    public async Task<IActionResult> Complete(long id, [FromBody] long completedBy, CancellationToken ct)
    {
        var result = await mediator.Send(new CompleteSelfAssessmentCommand(id, completedBy), ct);
        if (!result) return NotFound();
        return NoContent();
    }
}
