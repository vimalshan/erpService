using LeaveServices.Application.Features.LeaveEncashments.Commands;
using LeaveServices.Application.Features.LeaveEncashments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class EncashmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public EncashmentsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get encashment by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetEncashmentByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get encashments for an employee, optionally filtered by status.</summary>
    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId, [FromQuery] char? status, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetEncashmentsByEmployeeQuery(empSysId, status), ct);
        return Ok(result);
    }

    /// <summary>Apply for leave encashment.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Apply([FromBody] ApplyLeaveEncashmentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.EncashmentId }, result);
    }

    /// <summary>Update encashment status (Approve / Reject / Process).</summary>
    [HttpPatch("{id:long}/status")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateEncashmentStatusRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateEncashmentStatusCommand(id, req.NewStatus, req.ModifiedBy), ct);
        return Ok(result);
    }
}

public record UpdateEncashmentStatusRequest(char NewStatus, long ModifiedBy);
