using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparshTransactional.Application.Commands;
using SparshTransactional.Application.DTOs;
using SparshTransactional.Application.Queries;

namespace SparshTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DisbursementsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ScholarshipDisbursementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllDisbursementsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ScholarshipDisbursementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDisbursementByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("application/{applicationId:long}")]
    [ProducesResponseType(typeof(IReadOnlyList<ScholarshipDisbursementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByApplication(long applicationId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDisbursementsByApplicationQuery(applicationId), ct);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IReadOnlyList<ScholarshipDisbursementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStatus(string status, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDisbursementsByStatusQuery(status), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ScholarshipDisbursementDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateDisbursementCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DisbursementId }, result);
    }

    [HttpPost("{id:long}/complete")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ScholarshipDisbursementDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Complete(long id, [FromBody] CompleteDisbursementCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { DisbursementId = id }, ct);
        return Ok(result);
    }
}
