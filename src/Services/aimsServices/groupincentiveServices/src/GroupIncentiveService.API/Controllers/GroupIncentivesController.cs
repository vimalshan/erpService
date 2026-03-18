using GroupIncentiveService.Application.Commands.ApproveGroupIncentive;
using GroupIncentiveService.Application.Commands.CreateGroupIncentive;
using GroupIncentiveService.Application.Commands.RejectGroupIncentive;
using GroupIncentiveService.Application.Queries.GetGroupIncentiveById;
using GroupIncentiveService.Application.Queries.GetGroupIncentives;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupIncentiveService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class GroupIncentivesController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupIncentivesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets all pending incentives.</summary>
    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPendingIncentivesQuery(), ct);
        return Ok(result);
    }

    /// <summary>Gets incentives for a specific group.</summary>
    [HttpGet("group/{groupId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByGroup(int groupId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetGroupIncentivesQuery(groupId), ct);
        return Ok(result);
    }

    /// <summary>Gets a specific incentive by ID with details.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetGroupIncentiveByIdQuery(id), ct);
        return Ok(result);
    }

    /// <summary>Creates a new group incentive with detail lines.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateGroupIncentiveCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { IncentiveId = id });
    }

    /// <summary>Approves a group incentive.</summary>
    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "Approver,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveGroupIncentiveCommand command, CancellationToken ct)
    {
        await _mediator.Send(command with { IncentiveId = id }, ct);
        return NoContent();
    }

    /// <summary>Rejects a group incentive.</summary>
    [HttpPost("{id:long}/reject")]
    [Authorize(Roles = "Approver,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectGroupIncentiveCommand command, CancellationToken ct)
    {
        await _mediator.Send(command with { IncentiveId = id }, ct);
        return NoContent();
    }
}
