using BankService.Application.Commands.BankAccounts;
using BankService.Application.DTOs;
using BankService.Application.Queries.BankAccounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BankAccountsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BankAccountDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllBankAccountsQuery(), ct));

    [HttpGet("{accountId:long}")]
    public async Task<ActionResult<BankAccountDto>> GetById(long accountId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBankAccountByIdQuery(accountId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("trust/{trustCode}")]
    public async Task<ActionResult<IReadOnlyList<BankAccountDto>>> GetByTrustCode(string trustCode, CancellationToken ct)
        => Ok(await mediator.Send(new GetBankAccountsByTrustCodeQuery(trustCode), ct));

    [HttpPost]
    public async Task<ActionResult<BankAccountDto>> Create([FromBody] CreateBankAccountCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { accountId = result.AccountId }, result);
    }
}
