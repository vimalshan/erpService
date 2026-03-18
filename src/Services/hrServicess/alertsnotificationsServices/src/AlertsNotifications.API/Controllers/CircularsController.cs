using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Application.Features.Circulars.Commands;
using AlertsNotifications.Application.Features.Circulars.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlertsNotifications.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CircularsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CircularsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CircularDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllCircularsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CircularDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetCircularByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<CircularDto>>> GetByStatus(char status)
    {
        var result = await _mediator.Send(new GetCircularsByStatusQuery(status));
        return Ok(result);
    }

    [HttpGet("org/{orgId}")]
    public async Task<ActionResult<IEnumerable<CircularDto>>> GetByOrgId(long orgId)
    {
        var result = await _mediator.Send(new GetCircularsByOrgIdQuery(orgId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CircularDto>> Create([FromBody] CreateCircularCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.CircularId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCircularCommand command)
    {
        if (id != command.CircularId)
            return BadRequest("ID mismatch.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveCircularCommand command)
    {
        if (id != command.CircularId)
            return BadRequest("ID mismatch.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteCircularCommand(id));
        return NoContent();
    }
}
