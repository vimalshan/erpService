using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevelopmentService.Application.Commands.CreateLearningPlan;
using DevelopmentService.Application.Commands.ApprovePlan;
using DevelopmentService.Application.DTOs;
using DevelopmentService.Application.Queries.GetPlans;

namespace DevelopmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LearningPlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearningPlansController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets all learning plans, optionally filtered by userId and status.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LetPlanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? userId, [FromQuery] char? status, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPlansQuery(userId, status), ct);
        return Ok(result);
    }

    /// <summary>Creates a new learning and development plan.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(LetPlanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLearningPlanCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { userId = result.UserId }, result);
    }

    /// <summary>Approves or rejects a learning plan.</summary>
    [HttpPatch("{reqNum:long}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(
        long reqNum, [FromBody] ApprovePlanCommand command, CancellationToken ct)
    {
        var actualCommand = command with { ReqNum = reqNum };
        var updated = await _mediator.Send(actualCommand, ct);
        return updated ? NoContent() : NotFound();
    }
}
