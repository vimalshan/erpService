using AttendanceService.Application.Commands.AttendanceBatch;
using AttendanceService.Application.DTOs;
using AttendanceService.Application.Queries.AttendanceBatch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceService.API.Controllers;

[ApiController]
[Route("api/batches")]
[Authorize(Roles = "Admin,Hr")]
public class AttendanceBatchController(IMediator mediator) : ControllerBase
{
    /// <summary>Process monthly attendance and create a batch.</summary>
    [HttpPost("process")]
    [ProducesResponseType(typeof(AttendanceBatchDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Process([FromBody] ProcessMonthlyAttendanceRequest request, CancellationToken ct)
    {
        var cmd = new ProcessMonthlyAttendanceCommand(request.MonthStart, request.MonthEnd, request.ProcessedBy);
        var result = await mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(Get), new { id = result.BatchId }, result);
    }

    /// <summary>Get a batch by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AttendanceBatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAttendanceBatchQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }
}
