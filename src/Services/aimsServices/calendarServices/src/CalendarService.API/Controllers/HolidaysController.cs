using CalendarService.Application.Features.Holidays.Commands;
using CalendarService.Application.Features.Holidays.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HolidaysController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllHolidaysQuery(), ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await mediator.Send(new GetHolidayByIdQuery(id), ct));

    [HttpGet("range")]
    public async Task<IActionResult> GetByRange([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await mediator.Send(new GetHolidaysByDateRangeQuery(from, to), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHolidayCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateHolidayCommand cmd, CancellationToken ct)
        => Ok(await mediator.Send(cmd with { Id = id }, ct));
}
