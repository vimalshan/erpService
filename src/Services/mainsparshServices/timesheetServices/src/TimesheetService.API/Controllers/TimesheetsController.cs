using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimesheetService.Application.Commands.ApproveTimesheet;
using TimesheetService.Application.Commands.CreateTimesheet;
using TimesheetService.Application.Commands.RejectTimesheet;
using TimesheetService.Application.Commands.SubmitTimesheet;
using TimesheetService.Application.DTOs;
using TimesheetService.Application.Queries.GetPendingTimesheets;
using TimesheetService.Application.Queries.GetTimesheetById;
using TimesheetService.Application.Queries.GetTimesheetsByEmployee;

namespace TimesheetService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class TimesheetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimesheetsController(IMediator mediator) => _mediator = mediator;

    // GET api/timesheets/{id}
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TimesheetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTimesheetByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // GET api/timesheets/employee/{employeeId}
    [HttpGet("employee/{employeeId:long}")]
    [ProducesResponseType(typeof(IEnumerable<TimesheetSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(
        long employeeId,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTimesheetsByEmployeeQuery(employeeId, from, to), ct);
        return Ok(result);
    }

    // GET api/timesheets/pending
    [HttpGet("pending")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(typeof(IEnumerable<TimesheetSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPendingTimesheetsQuery(), ct);
        return Ok(result);
    }

    // POST api/timesheets
    [HttpPost]
    [ProducesResponseType(typeof(TimesheetDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTimesheetRequest request, CancellationToken ct)
    {
        var command = new CreateTimesheetCommand(
            request.EmployeeId,
            request.TimesheetDate,
            request.WorkDate,
            request.StartTime,
            request.EndTime,
            request.TotalHours,
            request.ProjectId,
            request.TaskId,
            request.WorkDescription,
            request.CreatedBy);

        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.TimesheetId }, result);
    }

    // POST api/timesheets/{id}/submit
    [HttpPost("{id:long}/submit")]
    [ProducesResponseType(typeof(TimesheetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Submit(long id, [FromBody] ActionByRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new SubmitTimesheetCommand(id, request.ActorId), ct);
        return Ok(result);
    }

    // POST api/timesheets/{id}/approve
    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(typeof(TimesheetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(long id, [FromBody] ActionByRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new ApproveTimesheetCommand(id, request.ActorId), ct);
        return Ok(result);
    }

    // POST api/timesheets/{id}/reject
    [HttpPost("{id:long}/reject")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(typeof(TimesheetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectTimesheetRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new RejectTimesheetCommand(id, request.ActorId, request.RejectionReason), ct);
        return Ok(result);
    }
}

// ── Request models ──────────────────────────────────────────────────────────
public sealed record CreateTimesheetRequest(
    long     EmployeeId,
    DateOnly TimesheetDate,
    DateOnly WorkDate,
    TimeOnly?StartTime,
    TimeOnly?EndTime,
    decimal? TotalHours,
    long?    ProjectId,
    long?    TaskId,
    string?  WorkDescription,
    long     CreatedBy);

public sealed record ActionByRequest(long ActorId);

public sealed record RejectTimesheetRequest(long ActorId, string RejectionReason);
