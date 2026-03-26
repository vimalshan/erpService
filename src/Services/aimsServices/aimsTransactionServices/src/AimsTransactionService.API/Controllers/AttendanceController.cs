using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AimsTransactionService.Application.Attendance.Commands.ProcessAttendanceBatch;
using AimsTransactionService.Application.Attendance.Queries.GetAttendanceSummary;

namespace AimsTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class AttendanceController(ISender sender) : ControllerBase
{
    [HttpGet("summary/{employeeSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSummary(
        long employeeSysId, [FromQuery] DateTime monthStart, [FromQuery] DateTime monthEnd, CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetAttendanceSummaryQuery(employeeSysId, monthStart, monthEnd), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("batch")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessBatch([FromBody] ProcessAttendanceBatchCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
