using CalendarService.Application.Features.Calendars.Commands;
using CalendarService.Application.Features.Calendars.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CalendarsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllCalendarsQuery(), ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await mediator.Send(new GetCalendarByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCalendarCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCalendarCommand cmd, CancellationToken ct)
        => Ok(await mediator.Send(cmd with { Id = id }, ct));

    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id, [FromBody] CloseCalendarCommand cmd, CancellationToken ct)
        => Ok(await mediator.Send(cmd with { Id = id }, ct));
}
