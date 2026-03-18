using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollServices.Application.Commands;
using PayrollServices.Application.Queries;

namespace PayrollServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PayrollTransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PayrollTransactionsController> _logger;

    public PayrollTransactionsController(IMediator mediator, ILogger<PayrollTransactionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("batch/{batchId}")]
    public async Task<ActionResult> GetTransactionsByBatch(long batchId)
    {
        var query = new GetPayrollTransactionsByBatchQuery { BatchId = batchId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult> GetEmployeePayroll(long employeeId, [FromQuery] string? month = null)
    {
        var query = new GetEmployeePayrollQuery { EmployeeSystemId = employeeId, Month = month };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateTransaction([FromBody] CreatePayrollTransactionCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetEmployeePayroll), new { employeeId = result.EmployeeSystemId }, result);
    }

    [HttpPut("{transactionId}/disburse")]
    public async Task<ActionResult> DisbursePayroll(long transactionId, [FromBody] DisbursePayrollCommand command)
    {
        command.TransactionId = transactionId;
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest("Failed to disburse payroll");

        return NoContent();
    }
}
