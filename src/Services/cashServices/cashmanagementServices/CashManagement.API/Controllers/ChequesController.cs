using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CashManagement.Application.Commands.ChequeRegister;
using CashManagement.Application.Queries.ChequeRegister;
using CashManagement.Application.DTOs;

namespace CashManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ChequesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ChequesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("by-account/{bankAccountId:long}")]
    [ProducesResponseType(typeof(IEnumerable<ChequeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAccount(long bankAccountId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetChequesByAccountQuery(bankAccountId), ct));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ChequeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetChequeByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ChequeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Issue([FromBody] IssueChequeCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ChequeId }, result);
    }

    [HttpPut("{id:long}/bounce")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Bounce(long id, [FromBody] MarkChequeBouncedCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command with { ChequeId = id }, ct);
        return result ? NoContent() : NotFound();
    }

    [HttpPut("{id:long}/clear")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Clear(long id, [FromBody] MarkChequeClearedCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command with { ChequeId = id }, ct);
        return result ? NoContent() : NotFound();
    }

    [HttpPut("{id:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long id, [FromBody] CancelChequeCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command with { ChequeId = id }, ct);
        return result ? NoContent() : NotFound();
    }
}
