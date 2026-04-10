using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSCTransactional.Application.Commands.Allocation;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Application.Queries.Allocation;

namespace SSCTransactional.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class AllocationsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AllocationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AllocationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllAllocationsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AllocationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllocationByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("doc/{docId:long}")]
    [ProducesResponseType(typeof(IEnumerable<AllocationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDocId(long docId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllocationsByDocIdQuery(docId), ct);
        return Ok(result);
    }

    [HttpGet("group/{groupId:long}")]
    [ProducesResponseType(typeof(IEnumerable<AllocationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByGroupId(long groupId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllocationsByGroupIdQuery(groupId), ct);
        return Ok(result);
    }

    [HttpGet("group/{groupId:long}/pending")]
    [ProducesResponseType(typeof(IEnumerable<AllocationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingByGroup(long groupId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetPendingAllocationsByGroupQuery(groupId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AllocationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAllocationCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.AllocationId }, result);
    }

    [HttpPut("{id:long}/pull")]
    [ProducesResponseType(typeof(AllocationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Pull(long id, [FromBody] PullAllocationCommand command, CancellationToken ct = default)
    {
        var cmd = command with { AllocationId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }

    [HttpPut("{id:long}/complete")]
    [ProducesResponseType(typeof(AllocationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Complete(long id, [FromBody] CompleteAllocationCommand command, CancellationToken ct = default)
    {
        var cmd = command with { AllocationId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }

    [HttpPut("{id:long}/hold")]
    [ProducesResponseType(typeof(AllocationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SetHold(long id, [FromBody] SetHoldCommand command, CancellationToken ct = default)
    {
        var cmd = command with { AllocationId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }

    [HttpPut("{id:long}/release-hold")]
    [ProducesResponseType(typeof(AllocationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReleaseHold(long id, [FromBody] ReleaseHoldCommand command, CancellationToken ct = default)
    {
        var cmd = command with { AllocationId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }

    [HttpPut("{id:long}/defective")]
    [ProducesResponseType(typeof(AllocationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkDefective(long id, [FromBody] MarkDefectiveCommand command, CancellationToken ct = default)
    {
        var cmd = command with { AllocationId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }

    [HttpPut("{id:long}/forward")]
    [ProducesResponseType(typeof(AllocationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Forward(long id, [FromBody] ForwardAllocationCommand command, CancellationToken ct = default)
    {
        var cmd = command with { AllocationId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }

    [HttpPut("{id:long}/reject")]
    [ProducesResponseType(typeof(AllocationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectAllocationCommand command, CancellationToken ct = default)
    {
        var cmd = command with { AllocationId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }
}
