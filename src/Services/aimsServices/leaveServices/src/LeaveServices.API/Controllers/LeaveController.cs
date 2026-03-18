using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveServices.Application.Commands.Leave;
using LeaveServices.Application.Queries.Leave;

namespace LeaveServices.API.Controllers;

[ApiController]
[Route("api/leaves")]
[Authorize]
[Produces("application/json")]
public sealed class LeaveController : ControllerBase
{
    private readonly IMediator _mediator;
    public LeaveController(IMediator mediator) => _mediator = mediator;

    /// <summary>Apply for leave.</summary>
    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] ApplyLeaveCommand cmd, CancellationToken ct)
    {
        var id = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// <summary>Get leave application by ID.</summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLeaveDetailByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get all leave applications for an employee.</summary>
    [HttpGet("employee/{empId:long}")]
    public async Task<IActionResult> GetByEmployee(long empId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetLeavesByEmployeeQuery(empId), ct));

    /// <summary>Get all pending leave applications (approver/HR view).</summary>
    [HttpGet("pending")]
    [Authorize(Roles = "Admin,HR,Approver")]
    public async Task<IActionResult> GetPending(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetPendingLeavesQuery(), ct));

    /// <summary>Cancel a leave application.</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Cancel(long id, [FromQuery] long cancelledBy, CancellationToken ct)
    {
        await _mediator.Send(new CancelLeaveCommand(id, cancelledBy), ct);
        return NoContent();
    }

    /// <summary>Get leave balance for an employee and leave type.</summary>
    [HttpGet("balance/{empId:long}/{leaveTypeId:long}")]
    public async Task<IActionResult> GetBalance(long empId, long leaveTypeId, CancellationToken ct) =>
        Ok(new { balance = await _mediator.Send(new GetLeaveBalanceQuery(empId, leaveTypeId), ct) });

    /// <summary>Get all leave balances for an employee in a given year.</summary>
    [HttpGet("balance/{empId:long}/year/{year:int}")]
    public async Task<IActionResult> GetBalanceAll(long empId, int year, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetLeaveBalanceAllQuery(empId, year), ct));
}
