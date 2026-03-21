using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseStructure.Application.Commands.CreateZone;
using WarehouseStructure.Application.Commands.DeleteZone;
using WarehouseStructure.Application.Commands.UpdateZone;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Application.Queries.GetAllZones;
using WarehouseStructure.Application.Queries.GetZoneById;

namespace WarehouseStructure.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ZonesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ZonesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ZoneDto>>> GetAll([FromQuery] int? warehouseId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllZonesQuery(warehouseId), ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ZoneDto>> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetZoneByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ZoneDto>> Create([FromBody] CreateZoneDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateZoneCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ZoneId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ZoneDto>> Update(int id, [FromBody] UpdateZoneDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateZoneCommand(id, dto), ct);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteZoneCommand(id), ct);
        return NoContent();
    }
}
