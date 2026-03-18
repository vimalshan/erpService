using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Application.Features.AlertGroups.Commands;
using AlertsNotifications.Application.Features.AlertGroups.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlertsNotifications.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlertGroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AlertGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlertGroupDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllAlertGroupsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlertGroupDto>> GetById(decimal id)
    {
        var result = await _mediator.Send(new GetAlertGroupByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AlertGroupDto>> Create([FromBody] CreateAlertGroupCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.AlertGroupId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(decimal id, [FromBody] UpdateAlertGroupCommand command)
    {
        if (id != command.AlertGroupId)
            return BadRequest("ID mismatch.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(decimal id)
    {
        await _mediator.Send(new DeleteAlertGroupCommand(id));
        return NoContent();
    }
}
