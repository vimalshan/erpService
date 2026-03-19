using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BatchAndEnvelopeService.Application.Commands.Envelope;
using BatchAndEnvelopeService.Application.DTOs;
using BatchAndEnvelopeService.Application.Queries.Envelope;

namespace BatchAndEnvelopeService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EnvelopesController : ControllerBase
{
    private readonly IMediator _mediator;
    public EnvelopesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EnvelopeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllEnvelopesQuery(page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(EnvelopeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetEnvelopeByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("type/{envelopeType}")]
    [ProducesResponseType(typeof(IEnumerable<EnvelopeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByType(string envelopeType, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetEnvelopesByTypeQuery(envelopeType), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(EnvelopeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEnvelopeCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.EnvelopeId }, result);
    }

    [HttpPut("{id:long}/confirm")]
    [ProducesResponseType(typeof(EnvelopeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirm(long id, [FromBody] ConfirmEnvelopeRequest request, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ConfirmEnvelopeCommand(id, request.ConfirmedBy), ct);
        return Ok(result);
    }

    [HttpPut("{id:long}/cancel")]
    [ProducesResponseType(typeof(EnvelopeDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel(long id, [FromBody] CancelEnvelopeRequest request, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new CancelEnvelopeCommand(id, request.CancelledBy), ct);
        return Ok(result);
    }
}

public record ConfirmEnvelopeRequest(long ConfirmedBy);
public record CancelEnvelopeRequest(long CancelledBy);
