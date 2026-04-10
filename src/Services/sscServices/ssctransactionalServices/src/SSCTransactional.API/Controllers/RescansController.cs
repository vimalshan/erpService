using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSCTransactional.Application.Commands.Rescan;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Application.Queries.Rescan;

namespace SSCTransactional.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class RescansController : ControllerBase
{
    private readonly IMediator _mediator;
    public RescansController(IMediator mediator) => _mediator = mediator;

    [HttpGet("doc/{docId:long}")]
    [ProducesResponseType(typeof(IEnumerable<RescanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDocId(long docId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetRescansByDocIdQuery(docId), ct);
        return Ok(result);
    }

    [HttpGet("pending")]
    [ProducesResponseType(typeof(IEnumerable<RescanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetPendingRescansQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RescanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRescanCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return Created("", result);
    }

    [HttpPut("{id:long}/complete")]
    [ProducesResponseType(typeof(RescanDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Complete(long id, [FromBody] CompleteRescanCommand command, CancellationToken ct = default)
    {
        var cmd = command with { RescanId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }
}
