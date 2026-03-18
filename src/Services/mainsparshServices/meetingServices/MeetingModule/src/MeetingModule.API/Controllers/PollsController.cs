using MediatR;
using MeetingModule.Application.Commands.Polls;
using MeetingModule.Application.DTOs;
using MeetingModule.Application.Queries.Polls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingModule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PollsController(IMediator mediator) : ControllerBase
{
    [HttpGet("meeting/{meetingId:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<PollDetailDto>>> GetByMeeting(long meetingId)
    {
        var result = await mediator.Send(new GetPollsByMeetingIdQuery(meetingId));
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<PollDetailDto>> GetById(long id)
    {
        var result = await mediator.Send(new GetPollByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PollDetailDto>> Create([FromBody] CreatePollDetailDto dto)
    {
        var result = await mediator.Send(new CreatePollCommand(dto, GetUserId()));
        return CreatedAtAction(nameof(GetById), new { id = result.PollId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<PollDetailDto>> Update(long id, [FromBody] UpdatePollDetailDto dto)
    {
        var result = await mediator.Send(new UpdatePollCommand(id, dto, GetUserId()));
        return Ok(result);
    }

    [HttpPut("{id:long}/close")]
    public async Task<IActionResult> Close(long id)
    {
        await mediator.Send(new ClosePollCommand(id, GetUserId()));
        return NoContent();
    }

    private long GetUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is not null && long.TryParse(claim.Value, out var id) ? id : 1;
    }
}
