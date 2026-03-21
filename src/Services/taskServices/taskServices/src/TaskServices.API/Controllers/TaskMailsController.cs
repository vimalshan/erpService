using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskServices.Application.DTOs;
using TaskServices.Application.Features.TaskMails.Commands;
using TaskServices.Application.Features.TaskMails.Queries;

namespace TaskServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskMailsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskMailsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TaskMailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTaskMailsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{mid}")]
    [ProducesResponseType(typeof(TaskMailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal mid, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTaskMailByIdQuery(mid), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("user/{sysId}")]
    [ProducesResponseType(typeof(IReadOnlyList<TaskMailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySystemUser(decimal sysId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTaskMailsBySystemUserQuery(sysId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaskMailCommand command, CancellationToken cancellationToken)
    {
        var mid = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { mid }, mid);
    }

    [HttpPut("{mid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(decimal mid, [FromBody] UpdateTaskMailCommand command, CancellationToken cancellationToken)
    {
        if (mid != command.MID)
            return BadRequest("MID in URL does not match body.");

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{mid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(decimal mid, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTaskMailCommand(mid), cancellationToken);
        return NoContent();
    }
}
