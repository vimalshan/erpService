using BankService.Application.Commands.BankMasters;
using BankService.Application.DTOs;
using BankService.Application.Queries.BankMasters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BankMastersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BankMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllBankMastersQuery(), ct));

    [HttpGet("{trustCode}/{bankCode}")]
    public async Task<ActionResult<BankMasterDto>> GetByCode(string trustCode, string bankCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBankMasterByCodeQuery(trustCode, bankCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("trust/{trustCode}")]
    public async Task<ActionResult<IReadOnlyList<BankMasterDto>>> GetByTrustCode(string trustCode, CancellationToken ct)
        => Ok(await mediator.Send(new GetBankMastersByTrustCodeQuery(trustCode), ct));

    [HttpPost]
    public async Task<ActionResult<BankMasterDto>> Create([FromBody] CreateBankMasterCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByCode),
            new { trustCode = result.BankTrustCode, bankCode = result.BankCode }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBankMasterCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result ? NoContent() : NotFound();
    }
}
