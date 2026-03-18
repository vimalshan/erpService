using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CashManagement.Application.Commands.BankReconciliation;
using CashManagement.Application.Queries.BankReconciliation;
using CashManagement.Application.DTOs;

namespace CashManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ReconciliationController : ControllerBase
{
    private readonly IMediator _mediator;
    public ReconciliationController(IMediator mediator) => _mediator = mediator;

    [HttpGet("by-account/{bankAccountId:long}")]
    [ProducesResponseType(typeof(IEnumerable<BankReconciliationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(long bankAccountId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetReconciliationHistoryQuery(bankAccountId), ct));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(BankReconciliationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetReconciliationByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BankReconciliationDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Perform([FromBody] PerformBankReconciliationCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ReconId }, result);
    }
}
