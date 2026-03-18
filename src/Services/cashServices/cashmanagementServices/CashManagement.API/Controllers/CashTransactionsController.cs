using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CashManagement.Application.Commands.CashTransaction;
using CashManagement.Application.Queries.CashUnit;
using CashManagement.Application.DTOs;

namespace CashManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CashTransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    public CashTransactionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("by-unit/{cashUnitId:long}")]
    [ProducesResponseType(typeof(IEnumerable<CashTransactionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUnit(long cashUnitId,
        [FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken ct)
    {
        var fromDate = from ?? DateTime.UtcNow.AddMonths(-1);
        var toDate = to ?? DateTime.UtcNow;
        return Ok(await _mediator.Send(new GetCashTransactionsByUnitQuery(cashUnitId, fromDate, toDate), ct));
    }

    [HttpPost("receipt")]
    [ProducesResponseType(typeof(CashTransactionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Receipt([FromBody] RecordCashReceiptCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("disbursement")]
    [ProducesResponseType(typeof(CashTransactionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Disbursement([FromBody] RecordCashDisbursementCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
