using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeService.Application.Commands.RecordTimeInfo;
using EmployeeService.Application.Queries.GetTimeInfo;

namespace EmployeeService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class TimeInfoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimeInfoController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all time-info records for an employee.</summary>
    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTimeInfoByEmployeeQuery(empSysId), ct);
        return Ok(result);
    }

    /// <summary>Get a single time-info record by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTimeInfoByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Record a new time-info / attendance flag.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Record([FromBody] RecordTimeInfoCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.TimeInfoId }, result);
    }
}
