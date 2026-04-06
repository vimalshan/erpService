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
public class PayTransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PayTransactionsController> _logger;

    public PayTransactionsController(IMediator mediator, ILogger<PayTransactionsController> logger)
    { _mediator = mediator; _logger = logger; }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PayTransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PayTransactionDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetPayTransactionByIdQuery(id));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpGet("employee/{employeeSystemId}")]
    [ProducesResponseType(typeof(IEnumerable<PayTransactionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PayTransactionDto>>> GetByEmployee(long employeeSystemId)
    {
        var result = await _mediator.Send(new GetPayTransactionsByEmployeeQuery(employeeSystemId));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpGet("month/{monthYear}")]
    [ProducesResponseType(typeof(IEnumerable<PayTransactionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PayTransactionDto>>> GetByMonth(string monthYear)
    {
        var result = await _mediator.Send(new GetPayTransactionsByMonthQuery(monthYear));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpGet("batch/{batchId}")]
    [ProducesResponseType(typeof(IEnumerable<PayTransactionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PayTransactionDto>>> GetByBatch(long batchId)
    {
        var result = await _mediator.Send(new GetPayTransactionsByBatchQuery(batchId));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpPost]
    [ProducesResponseType(typeof(PayTransactionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PayTransactionDto>> Create([FromBody] CreatePayTransactionDto dto)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var result = await _mediator.Send(new CreatePayTransactionCommand(dto, userId));
        if (!result.IsSuccess) return BadRequest(new { message = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
    }

    [HttpPost("{id}/complete")]
    [ProducesResponseType(typeof(PayTransactionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PayTransactionDto>> Complete(long id)
    {
        var result = await _mediator.Send(new CompletePayTransactionCommand(id));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.Error });
    }

    [HttpPost("{id}/revoke")]
    [ProducesResponseType(typeof(PayTransactionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PayTransactionDto>> Revoke(long id, [FromQuery] string? reason = null)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var result = await _mediator.Send(new RevokePayTransactionCommand(id, userId, reason));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.Error });
    }
}
