using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RackingSystem.Application.Features.Bins.Commands;
using RackingSystem.Application.Features.Bins.DTOs;
using RackingSystem.Application.Features.Bins.Queries;

namespace RackingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class BinsController : ControllerBase
{
    private readonly IMediator _mediator;
    public BinsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BinDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? zoneId, [FromQuery] int? shelfId,
        [FromQuery] string? status, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetBinsQuery(zoneId, shelfId, status), ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BinDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetBinByIdQuery(id), ct));

    [HttpGet("barcode/{barcode}")]
    [ProducesResponseType(typeof(BinDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBarcode(string barcode, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetBinByBarcodeQuery(barcode), ct));

    [HttpPost]
    [ProducesResponseType(typeof(BinDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBinCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BinDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBinCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest("Route id and body id must match.");
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(BinDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBinStatusRequest request, CancellationToken ct) =>
        Ok(await _mediator.Send(new UpdateBinStatusCommand(id, request.NewStatus), ct));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteBinCommand(id), ct);
        return NoContent();
    }
}

public record UpdateBinStatusRequest(string NewStatus);
