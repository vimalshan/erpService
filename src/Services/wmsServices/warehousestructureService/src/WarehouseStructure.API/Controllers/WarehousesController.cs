using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseStructure.Application.Commands.CreateWarehouse;
using WarehouseStructure.Application.Commands.DeleteWarehouse;
using WarehouseStructure.Application.Commands.UpdateWarehouse;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Application.Queries.GetAllWarehouses;
using WarehouseStructure.Application.Queries.GetWarehouseById;

namespace WarehouseStructure.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehousesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WarehouseDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllWarehousesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WarehouseDto>> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWarehouseByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WarehouseDto>> Create([FromBody] CreateWarehouseDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateWarehouseCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.WarehouseId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WarehouseDto>> Update(int id, [FromBody] UpdateWarehouseDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateWarehouseCommand(id, dto), ct);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteWarehouseCommand(id), ct);
        return NoContent();
    }
}
