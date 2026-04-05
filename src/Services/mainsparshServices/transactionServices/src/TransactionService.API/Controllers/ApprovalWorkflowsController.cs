using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.DTOs;
using TransactionService.Application.Features.ApprovalWorkflows.Commands;
using TransactionService.Application.Features.ApprovalWorkflows.Queries;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/v1/workflows")]
[Authorize]
public class ApprovalWorkflowsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalWorkflowsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ApprovalWorkflowDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllWorkflowsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApprovalWorkflowDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWorkflowByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-code/{code}")]
    [ProducesResponseType(typeof(ApprovalWorkflowDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCode(string code, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWorkflowByCodeQuery(code), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("pending/{approverId:long}")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalWorkflowDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(long approverId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPendingWorkflowsQuery(approverId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("by-entity")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalWorkflowDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEntity([FromQuery] string entityType, [FromQuery] long entityId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWorkflowsByEntityQuery(entityType, entityId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApprovalWorkflowDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Submit([FromBody] SubmitWorkflowCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.WorkflowId }, result);
    }

    [HttpPost("{id:long}/approve")]
    [ProducesResponseType(typeof(ApprovalWorkflowDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveStepCommand command, CancellationToken cancellationToken)
    {
        if (id != command.WorkflowId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:long}/reject")]
    [ProducesResponseType(typeof(ApprovalWorkflowDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectStepCommand command, CancellationToken cancellationToken)
    {
        if (id != command.WorkflowId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel(long id, [FromBody] CancelWorkflowCommand command, CancellationToken cancellationToken)
    {
        if (id != command.WorkflowId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
