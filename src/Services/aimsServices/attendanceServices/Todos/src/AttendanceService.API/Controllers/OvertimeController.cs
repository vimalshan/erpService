using AttendanceService.Application.Commands.Overtime;
using AttendanceService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceService.API.Controllers;

[ApiController]
[Route("api/overtime")]
[Authorize]
public class OvertimeController(IMediator mediator) : ControllerBase
{
    /// <summary>Approve an overtime record.</summary>
    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "Admin,Hr")]
    [ProducesResponseType(typeof(OvertimeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(long id, [FromQuery] long approvedBy, CancellationToken ct)
    {
        var result = await mediator.Send(new ApproveOvertimeCommand(id, approvedBy), ct);
        return Ok(result);
    }
}
