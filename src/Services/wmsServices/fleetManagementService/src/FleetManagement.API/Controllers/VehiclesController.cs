using FleetManagement.Application.Commands.Vehicles;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Queries.Vehicles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllVehiclesQuery(), ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehicleDto>> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetVehicleByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<VehicleDto>>> GetByStatus(string status, CancellationToken ct)
        => Ok(await mediator.Send(new GetVehiclesByStatusQuery(status), ct));

    [HttpPost]
    public async Task<ActionResult<VehicleDto>> Create([FromBody] CreateVehicleCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.VehicleId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VehicleDto>> Update(int id, [FromBody] UpdateVehicleCommand command, CancellationToken ct)
    {
        if (id != command.VehicleId) return BadRequest();
        return Ok(await mediator.Send(command, ct));
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<VehicleDto>> ChangeStatus(int id, [FromBody] ChangeVehicleStatusCommand command, CancellationToken ct)
    {
        if (id != command.VehicleId) return BadRequest();
        return Ok(await mediator.Send(command, ct));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteVehicleCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}
