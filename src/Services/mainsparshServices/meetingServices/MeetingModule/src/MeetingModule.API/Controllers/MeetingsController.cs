using MediatR;
using MeetingModule.Application.Commands.Meetings;
using MeetingModule.Application.DTOs;
using MeetingModule.Application.Queries.Meetings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingModule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeetingsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MeetingScheduleDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllMeetingsQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<MeetingScheduleDto>> GetById(long id)
    {
        var result = await mediator.Send(new GetMeetingByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("status/{status}")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MeetingScheduleDto>>> GetByStatus(string status)
    {
        var result = await mediator.Send(new GetMeetingsByStatusQuery(status));
        return Ok(result);
    }

    [HttpGet("organizer/{organizerId:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MeetingScheduleDto>>> GetByOrganizer(long organizerId)
    {
        var result = await mediator.Send(new GetMeetingsByOrganizerQuery(organizerId));
        return Ok(result);
    }

    [HttpGet("daterange")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MeetingScheduleDto>>> GetByDateRange(
        [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await mediator.Send(new GetMeetingsByDateRangeQuery(from, to));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MeetingScheduleDto>> Create([FromBody] CreateMeetingScheduleDto dto)
    {
        var result = await mediator.Send(new CreateMeetingCommand(dto, GetUserId()));
        return CreatedAtAction(nameof(GetById), new { id = result.MeetingId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<MeetingScheduleDto>> Update(long id, [FromBody] UpdateMeetingScheduleDto dto)
    {
        var result = await mediator.Send(new UpdateMeetingCommand(id, dto, GetUserId()));
        return Ok(result);
    }

    [HttpPut("{id:long}/start")]
    public async Task<IActionResult> Start(long id)
    {
        await mediator.Send(new StartMeetingCommand(id, GetUserId()));
        return NoContent();
    }

    [HttpPut("{id:long}/complete")]
    public async Task<IActionResult> Complete(long id)
    {
        await mediator.Send(new CompleteMeetingCommand(id, GetUserId()));
        return NoContent();
    }

    [HttpPut("{id:long}/cancel")]
    public async Task<IActionResult> Cancel(long id)
    {
        await mediator.Send(new CancelMeetingCommand(id, GetUserId()));
        return NoContent();
    }

    private long GetUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is not null && long.TryParse(claim.Value, out var id) ? id : 1;
    }
}
