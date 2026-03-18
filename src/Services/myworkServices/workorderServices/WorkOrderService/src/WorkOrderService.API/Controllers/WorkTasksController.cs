using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrderService.Application.Commands.AssignTask;
using WorkOrderService.Application.Commands.CompleteTask;
using WorkOrderService.Application.DTOs;
using WorkOrderService.Application.Queries.GetTasksByWorkOrder;

namespace WorkOrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkTasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkTasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("by-workorder/{workOrderId:long}")]
    [ProducesResponseType(typeof(IEnumerable<WorkTaskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WorkTaskDto>>> GetByWorkOrder(long workOrderId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTasksByWorkOrderQuery(workOrderId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(WorkTaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WorkTaskDto>> AssignTask([FromBody] AssignTaskCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }

    [HttpPut("complete")]
    [ProducesResponseType(typeof(WorkTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkTaskDto>> CompleteTask([FromBody] CompleteTaskCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
