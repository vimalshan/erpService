using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BatchAndEnvelopeService.Application.Commands.Batch;
using BatchAndEnvelopeService.Application.DTOs;
using BatchAndEnvelopeService.Application.Queries.Batch;

namespace BatchAndEnvelopeService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class BatchesController : ControllerBase
{
    private readonly IMediator _mediator;
    public BatchesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BatchDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllBatchesQuery(page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(BatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetBatchByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("location/{locationId:long}")]
    [ProducesResponseType(typeof(IEnumerable<BatchDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByLocation(long locationId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetBatchesByLocationQuery(locationId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BatchDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBatchCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.BatchId }, result);
    }

    [HttpPut("{id:long}/confirm")]
    [ProducesResponseType(typeof(BatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirm(long id, [FromBody] ConfirmBatchRequest request, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ConfirmBatchCommand(id, request.ConfirmedBy), ct);
        return Ok(result);
    }

    [HttpPut("{id:long}/cancel")]
    [ProducesResponseType(typeof(BatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long id, [FromBody] CancelBatchRequest request, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new CancelBatchCommand(id, request.CancelledBy), ct);
        return Ok(result);
    }
}

public record ConfirmBatchRequest(long ConfirmedBy);
public record CancelBatchRequest(long CancelledBy);
