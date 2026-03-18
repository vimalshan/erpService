using AuditService.Application.Commands.Observations;
using AuditService.Application.DTOs;
using AuditService.Application.Queries.Observations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ObservationController : ControllerBase
{
    private readonly ISender _sender;

    public ObservationController(ISender sender) => _sender = sender;

    /// <summary>Returns all pending observations.</summary>
    [HttpGet("pending")]
    [ProducesResponseType(typeof(IEnumerable<ObservationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPendingObservationsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Returns observation by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ObservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetObservationByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Returns observations for a specific audit.</summary>
    [HttpGet("audit/{auditId:long}")]
    [ProducesResponseType(typeof(IEnumerable<ObservationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAudit(long auditId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetObservationsByAuditQuery(auditId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Creates a new observation.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ObservationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateObservationRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateObservationCommand(
            request.ObvId, request.AuditId, request.Title, request.Description,
            request.Risk, request.Auditee, request.Esc1, request.Esc2,
            request.ManComments, request.OrgDueDate, request.Location,
            request.AuditorName, request.Remarks, request.CreatedBy);

        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.ObvId }, result);
    }

    /// <summary>Updates observation status (P/R/C).</summary>
    [HttpPatch("{id:long}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateObservationStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateObservationStatusCommand(id, request.NewStatus, request.ModifiedBy);
        var success = await _sender.Send(command, cancellationToken);
        return success ? NoContent() : NotFound();
    }
}
