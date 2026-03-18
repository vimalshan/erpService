using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollServices.Application.Commands;
using PayrollServices.Application.Queries;

namespace PayrollServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayrollBatchesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PayrollBatchesController> _logger;

    public PayrollBatchesController(IMediator mediator, ILogger<PayrollBatchesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{batchId}")]
    public async Task<ActionResult> GetBatchById(long batchId)
    {
        var query = new GetPayrollBatchByIdQuery { BatchId = batchId };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("by-month/{month}")]
    public async Task<ActionResult> GetBatchByMonth(string month)
    {
        var query = new GetPayrollBatchByMonthQuery { Month = month };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllBatches()
    {
        var query = new GetAllPayrollBatchesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateBatch([FromBody] CreatePayrollBatchCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBatchById), new { batchId = result.BatchId }, result);
    }

    [HttpPost("process-monthly-salary")]
    public async Task<ActionResult> ProcessMonthlySalary([FromBody] ProcessMonthlySalaryCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
