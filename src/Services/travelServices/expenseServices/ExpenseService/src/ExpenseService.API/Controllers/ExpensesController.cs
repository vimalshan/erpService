using ExpenseService.Application.Commands;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get expense by request number and serial number
    /// </summary>
    [HttpGet("{requestNumber}/{serialNumber}")]
    [ProducesResponseType(typeof(TravelExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long requestNumber, long serialNumber)
    {
        var result = await _mediator.Send(new GetExpenseByIdQuery
        {
            RequestNumber = requestNumber,
            SerialNumber = serialNumber
        });

        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Get all expenses for a travel request
    /// </summary>
    [HttpGet("request/{requestNumber}")]
    [ProducesResponseType(typeof(IReadOnlyList<TravelExpenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRequest(long requestNumber)
    {
        var result = await _mediator.Send(new GetExpensesByRequestQuery { RequestNumber = requestNumber });
        return Ok(result);
    }

    /// <summary>
    /// Record a new travel expense
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TravelExpenseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] RecordExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById),
            new { requestNumber = result.RequestNumber, serialNumber = result.SerialNumber }, result);
    }

    /// <summary>
    /// Delete an expense
    /// </summary>
    [HttpDelete("{requestNumber}/{serialNumber}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long requestNumber, long serialNumber)
    {
        var result = await _mediator.Send(new DeleteExpenseCommand
        {
            RequestNumber = requestNumber,
            SerialNumber = serialNumber
        });

        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Get expense summary for a request
    /// </summary>
    [HttpGet("summary/{requestNumber}")]
    [ProducesResponseType(typeof(ExpenseSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSummary(long requestNumber)
    {
        var result = await _mediator.Send(new GetExpenseSummaryQuery { RequestNumber = requestNumber });
        return result == null ? NotFound() : Ok(result);
    }
}
