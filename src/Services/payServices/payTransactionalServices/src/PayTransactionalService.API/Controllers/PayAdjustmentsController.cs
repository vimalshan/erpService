using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayTransactionalService.Application.Commands;
using PayTransactionalService.Application.DTOs;
using PayTransactionalService.Application.Queries;

namespace PayTransactionalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PayAdjustmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PayAdjustmentsController> _logger;

    public PayAdjustmentsController(IMediator mediator, ILogger<PayAdjustmentsController> logger)
    { _mediator = mediator; _logger = logger; }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PayAdjustmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PayAdjustmentDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetPayAdjustmentByIdQuery(id));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpGet("employee/{employeeSystemId}")]
    [ProducesResponseType(typeof(IEnumerable<PayAdjustmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PayAdjustmentDto>>> GetByEmployee(long employeeSystemId)
    {
        var result = await _mediator.Send(new GetPayAdjustmentsByEmployeeQuery(employeeSystemId));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpGet("pending")]
    [ProducesResponseType(typeof(IEnumerable<PayAdjustmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PayAdjustmentDto>>> GetPending()
    {
        var result = await _mediator.Send(new GetPendingAdjustmentsQuery());
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpPost]
    [ProducesResponseType(typeof(PayAdjustmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PayAdjustmentDto>> Create([FromBody] CreatePayAdjustmentDto dto)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var result = await _mediator.Send(new CreatePayAdjustmentCommand(dto, userId));
        if (!result.IsSuccess) return BadRequest(new { message = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
    }

    [HttpPost("{id}/approve")]
    [ProducesResponseType(typeof(PayAdjustmentDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PayAdjustmentDto>> Approve(long id, [FromQuery] long approvedBy)
    {
        var result = await _mediator.Send(new ApprovePayAdjustmentCommand(id, approvedBy));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.Error });
    }

    [HttpPost("{id}/reject")]
    [ProducesResponseType(typeof(PayAdjustmentDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PayAdjustmentDto>> Reject(long id, [FromQuery] long rejectedBy, [FromQuery] string? reason = null)
    {
        var result = await _mediator.Send(new RejectPayAdjustmentCommand(id, rejectedBy, reason));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.Error });
    }
}
