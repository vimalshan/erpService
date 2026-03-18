using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwipeTransactionService.Application.Features.CanteenPunch.Commands;
using SwipeTransactionService.Application.Features.CanteenPunch.Queries;

namespace SwipeTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CanteenPunchController : ControllerBase
{
    private readonly IMediator _mediator;

    public CanteenPunchController(IMediator mediator) => _mediator = mediator;

    /// <summary>Records a canteen punch (check-in or check-out).</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecordPunch(
        [FromBody] RecordPunchCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>Gets today's punch record for an employee.</summary>
    [HttpGet("{empSysId}/today")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTodayPunch(long empSysId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetPunchByEmployeeDateQuery(empSysId, DateTime.UtcNow.Date), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Gets punch records for an employee within a date range.</summary>
    [HttpGet("{empSysId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPunches(
        long empSysId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPunchesByEmployeeQuery(empSysId, from, to), ct);
        return Ok(result);
    }
}
