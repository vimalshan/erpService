using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategicStock.Application.Commands.CloseStrategicStock;
using StrategicStock.Application.Commands.CreateStrategicStock;
using StrategicStock.Application.Commands.UpdateStrategicStock;
using StrategicStock.Application.DTOs;
using StrategicStock.Application.Queries.GetAllStrategicStocks;
using StrategicStock.Application.Queries.GetStrategicStockById;
using StrategicStock.Application.Queries.GetStrategicStockInfo;

namespace StrategicStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class StrategicStockController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StrategicStockDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllStrategicStocksQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StrategicStockDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetStrategicStockByIdQuery(id), ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("info")]
    [ProducesResponseType(typeof(IReadOnlyList<StrategicStockInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInfo([FromQuery] int itemId, [FromQuery] int companyUnitId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetStrategicStockInfoQuery(itemId, companyUnitId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateStrategicStockCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStrategicStockCommand command, CancellationToken ct)
    {
        if (id != command.StrategicStockId)
            return BadRequest("ID mismatch.");

        await mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPost("{id:int}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Close(int id, [FromQuery] int? userId, CancellationToken ct)
    {
        await mediator.Send(new CloseStrategicStockCommand(id, userId), ct);
        return NoContent();
    }
}
