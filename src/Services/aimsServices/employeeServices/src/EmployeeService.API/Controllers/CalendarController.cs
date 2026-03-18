using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeService.Application.Commands.MapCalendar;
using EmployeeService.Application.Queries.GetCalendars;

namespace EmployeeService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class CalendarController : ControllerBase
{
    private readonly IMediator _mediator;
    public CalendarController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all calendar mappings for an employee.</summary>
    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCalendarsByEmployeeQuery(empSysId), ct);
        return Ok(result);
    }

    /// <summary>Map an employee to a calendar.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Map([FromBody] MapCalendarCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Created($"api/calendar/{result.EmpCalId}", result);
    }
}
