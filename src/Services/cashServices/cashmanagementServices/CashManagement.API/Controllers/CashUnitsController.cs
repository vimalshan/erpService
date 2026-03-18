using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CashManagement.Application.Commands.CashUnit;
using CashManagement.Application.Queries.CashUnit;
using CashManagement.Application.DTOs;

namespace CashManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CashUnitsController : ControllerBase
{
    private readonly IMediator _mediator;
    public CashUnitsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CashUnitDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllCashUnitsQuery(), ct));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(CashUnitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCashUnitByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:long}/balance")]
    [ProducesResponseType(typeof(CashBalanceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBalance(long id, [FromQuery] DateTime? asOfDate, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCashInHandQuery(id, asOfDate ?? DateTime.UtcNow), ct));

    [HttpPost]
    [ProducesResponseType(typeof(CashUnitDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCashUnitCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.CashUnitId }, result);
    }

    [HttpPut("{id:long}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateCashUnitStatusCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command with { CashUnitId = id }, ct);
        return result ? NoContent() : NotFound();
    }
}
