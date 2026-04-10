using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSCTransactional.Application.Commands.Correspondence;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Application.Queries.Correspondence;

namespace SSCTransactional.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CorrespondencesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CorrespondencesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(CorrespondenceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetCorrespondenceByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("doc/{docId:long}")]
    [ProducesResponseType(typeof(IEnumerable<CorrespondenceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDocId(long docId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetCorrespondencesByDocIdQuery(docId), ct);
        return Ok(result);
    }

    [HttpGet("holds")]
    [ProducesResponseType(typeof(IEnumerable<CorrespondenceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveHolds(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetActiveHoldsQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CorrespondenceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCorrespondenceCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.CorrespondenceId }, result);
    }

    [HttpPut("{id:long}/release")]
    [ProducesResponseType(typeof(CorrespondenceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Release(long id, [FromBody] ReleaseCorrespondenceCommand command, CancellationToken ct = default)
    {
        var cmd = command with { CorrespondenceId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }
}
