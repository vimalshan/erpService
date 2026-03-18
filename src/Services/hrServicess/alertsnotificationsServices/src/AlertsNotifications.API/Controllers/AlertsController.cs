using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Application.Features.Alerts.Commands;
using AlertsNotifications.Application.Features.Alerts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlertsNotifications.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlertsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AlertsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlertMasterDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllAlertsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlertMasterDto>> GetById(decimal id)
    {
        var result = await _mediator.Send(new GetAlertByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("app/{appName}")]
    public async Task<ActionResult<IEnumerable<AlertMasterDto>>> GetByApp(string appName)
    {
        var result = await _mediator.Send(new GetAlertsByAppQuery(appName));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AlertMasterDto>> Create([FromBody] CreateAlertCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.AlertId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(decimal id, [FromBody] UpdateAlertCommand command)
    {
        if (id != command.AlertId)
            return BadRequest("ID mismatch.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(decimal id)
    {
        await _mediator.Send(new DeleteAlertCommand(id));
        return NoContent();
    }
}
