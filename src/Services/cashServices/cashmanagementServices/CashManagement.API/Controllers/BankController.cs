using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CashManagement.Application.Commands.BankAccount;
using CashManagement.Application.Commands.BankTransaction;
using CashManagement.Application.Queries.BankAccount;
using CashManagement.Application.DTOs;

namespace CashManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class BankAccountsController : ControllerBase
{
    private readonly IMediator _mediator;
    public BankAccountsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BankAccountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllBankAccountsQuery(), ct));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(BankAccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBankAccountByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:long}/balance")]
    [ProducesResponseType(typeof(BankBalanceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBalance(long id, [FromQuery] DateTime? asOfDate, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBankBalanceQuery(id, asOfDate ?? DateTime.UtcNow), ct));

    [HttpPost]
    [ProducesResponseType(typeof(BankAccountDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBankAccountCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.BankAccountId }, result);
    }

    [HttpPut("{id:long}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateBankAccountStatusCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command with { BankAccountId = id }, ct);
        return result ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/v1/bank-transactions")]
[Authorize]
public class BankTransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    public BankTransactionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("by-account/{bankAccountId:long}")]
    [ProducesResponseType(typeof(IEnumerable<BankTransactionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAccount(long bankAccountId,
        [FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBankTransactionsByAccountQuery(
            bankAccountId, from ?? DateTime.UtcNow.AddMonths(-1), to ?? DateTime.UtcNow), ct));

    [HttpPost]
    [ProducesResponseType(typeof(BankTransactionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] RecordBankTransactionCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
