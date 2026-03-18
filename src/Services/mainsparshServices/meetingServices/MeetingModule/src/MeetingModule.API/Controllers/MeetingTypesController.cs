using MediatR;
using MeetingModule.Application.Commands.MeetingTypes;
using MeetingModule.Application.DTOs;
using MeetingModule.Application.Queries.MeetingTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingModule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeetingTypesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MeetingTypeDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllMeetingTypesQuery());
        return Ok(result);
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MeetingTypeDto>>> GetActive()
    {
        var result = await mediator.Send(new GetActiveMeetingTypesQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<MeetingTypeDto>> GetById(long id)
    {
        var result = await mediator.Send(new GetMeetingTypeByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("code/{code}")]
    [AllowAnonymous]
    public async Task<ActionResult<MeetingTypeDto>> GetByCode(string code)
    {
        var result = await mediator.Send(new GetMeetingTypeByCodeQuery(code));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MeetingTypeDto>> Create([FromBody] CreateMeetingTypeDto dto)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new CreateMeetingTypeCommand(dto, userId));
        return CreatedAtAction(nameof(GetById), new { id = result.MeetTypeId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<MeetingTypeDto>> Update(long id, [FromBody] UpdateMeetingTypeDto dto)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new UpdateMeetingTypeCommand(id, dto, userId));
        return Ok(result);
    }

    [HttpPut("{id:long}/activate")]
    public async Task<IActionResult> Activate(long id)
    {
        await mediator.Send(new ActivateMeetingTypeCommand(id, GetUserId()));
        return NoContent();
    }

    [HttpPut("{id:long}/deactivate")]
    public async Task<IActionResult> Deactivate(long id)
    {
        await mediator.Send(new DeactivateMeetingTypeCommand(id, GetUserId()));
        return NoContent();
    }

    private long GetUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is not null && long.TryParse(claim.Value, out var id) ? id : 1;
    }
}
