using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSCTransactional.Application.Commands.Approval;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Application.Queries.Approval;

namespace SSCTransactional.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ApprovalsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ApprovalsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("doc/{docId:long}")]
    [ProducesResponseType(typeof(IEnumerable<DocumentApprovalDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDocId(long docId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetApprovalsByDocIdQuery(docId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DocumentApprovalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateApprovalCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return Created("", result);
    }

    [HttpPut("{id:long}/status")]
    [ProducesResponseType(typeof(DocumentApprovalDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateApprovalStatusCommand command, CancellationToken ct = default)
    {
        var cmd = command with { ApprovalId = id };
        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }
}
