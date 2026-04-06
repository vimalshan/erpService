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
public class PayrollBatchesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PayrollBatchesController> _logger;

    public PayrollBatchesController(IMediator mediator, ILogger<PayrollBatchesController> logger)
    { _mediator = mediator; _logger = logger; }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PayrollBatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PayrollBatchDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetPayrollBatchByIdQuery(id));
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PayrollBatchDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PayrollBatchDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllPayrollBatchesQuery());
        return result.IsSuccess ? Ok(result.Data) : NotFound(new { message = result.Error });
    }

    [HttpPost("process")]
    [ProducesResponseType(typeof(PayrollBatchDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PayrollBatchDto>> ProcessMonthly([FromBody] ProcessMonthlySalaryDto dto)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var result = await _mediator.Send(new ProcessMonthlySalaryCommand(dto.MonthYear, userId));
        if (!result.IsSuccess) return BadRequest(new { message = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
    }

    [HttpPost("{id}/revoke")]
    [ProducesResponseType(typeof(PayrollBatchDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PayrollBatchDto>> Revoke(long id)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        var result = await _mediator.Send(new RevokePayrollBatchCommand(id, userId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.Error });
    }
}
