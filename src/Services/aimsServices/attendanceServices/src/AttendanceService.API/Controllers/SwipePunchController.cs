using AttendanceService.Application.Commands.SwipePunch;
using AttendanceService.Application.DTOs;
using AttendanceService.Application.Queries.Attendance;
using AttendanceService.Application.Queries.SwipePunch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceService.API.Controllers;

[ApiController]
[Route("api/swipe")]
[Authorize]
public class SwipePunchController(IMediator mediator) : ControllerBase
{
    /// <summary>Record a biometric swipe punch.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(SwipePunchDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Record([FromBody] RecordSwipePunchRequest request, CancellationToken ct)
    {
        var cmd = new RecordSwipePunchCommand(request.EmpSysId, request.PunchTime, request.GateNo, request.PunchStatus);
        var result = await mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetByEmployee), new { empSysId = result.EmpSysId }, result);
    }

    /// <summary>Get swipe punches for an employee, optionally filtered by date range.</summary>
    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<SwipePunchDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId,
        [FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSwipePunchesByEmployeeQuery(empSysId, from, to), ct);
        return Ok(result);
    }

    /// <summary>Get attendance percentage for an employee in a month.</summary>
    [HttpGet("employee/{empSysId:long}/percentage")]
    [ProducesResponseType(typeof(AttendancePercentageDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPercentage(long empSysId,
        [FromQuery] DateTime monthStart, [FromQuery] DateTime monthEnd, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAttendancePercentageQuery(empSysId, monthStart, monthEnd), ct);
        return Ok(result);
    }
}
