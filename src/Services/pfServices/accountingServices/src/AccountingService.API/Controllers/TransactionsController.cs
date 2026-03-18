using AccountingService.Application.Features.Transactions.Commands.CancelTransaction;
using AccountingService.Application.Features.Transactions.Commands.CreateTransaction;
using AccountingService.Application.Features.Transactions.Queries.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get transactions by trust code</summary>
    [HttpGet("{trustCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTrustCode(string trustCode, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTransactionsQuery(trustCode), ct));

    /// <summary>Create a new transaction</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTransactionCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByTrustCode),
            new { trustCode = result.TdTrustCode }, result);
    }

    /// <summary>Cancel a transaction</summary>
    [HttpDelete("{trustCode}/{transactionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        string trustCode, int transactionId,
        [FromQuery] string cancelledBy,
        CancellationToken ct)
    {
        await _mediator.Send(new CancelTransactionCommand(trustCode, transactionId, cancelledBy), ct);
        return NoContent();
    }
}
