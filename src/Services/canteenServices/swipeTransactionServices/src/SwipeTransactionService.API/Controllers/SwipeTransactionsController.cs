using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwipeTransactionService.Application.Features.SwipeTransactions.Commands;
using SwipeTransactionService.Application.Features.SwipeTransactions.Queries;

namespace SwipeTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SwipeTransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SwipeTransactionsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Records a new swipe card upload transaction.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecordSwipe(
        [FromBody] RecordSwipeUploadCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByEmployee), new { employeeNumber = result.EmployeeNumber }, result);
    }

    /// <summary>Gets swipe transactions for an employee within a date range.</summary>
    [HttpGet("{employeeNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(
        string employeeNumber,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetSwipesByEmployeeQuery(employeeNumber, from, to), ct);
        return Ok(result);
    }

    /// <summary>Gets all pending (unprocessed) swipe transactions.</summary>
    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPendingSwipesQuery(), ct);
        return Ok(result);
    }
}
