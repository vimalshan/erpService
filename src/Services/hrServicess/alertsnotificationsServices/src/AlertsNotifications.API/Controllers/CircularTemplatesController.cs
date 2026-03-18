using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Application.Features.CircularTemplates.Commands;
using AlertsNotifications.Application.Features.CircularTemplates.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlertsNotifications.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CircularTemplatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CircularTemplatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CircularTemplateDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllCircularTemplatesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CircularTemplateDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetCircularTemplateByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("type/{typeId}")]
    public async Task<ActionResult<IEnumerable<CircularTemplateDto>>> GetByType(long typeId)
    {
        var result = await _mediator.Send(new GetCircularTemplatesByTypeQuery(typeId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CircularTemplateDto>> Create([FromBody] CreateCircularTemplateCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.CircularTemplateId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCircularTemplateCommand command)
    {
        if (id != command.CircularTemplateId)
            return BadRequest("ID mismatch.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteCircularTemplateCommand(id));
        return NoContent();
    }
}
