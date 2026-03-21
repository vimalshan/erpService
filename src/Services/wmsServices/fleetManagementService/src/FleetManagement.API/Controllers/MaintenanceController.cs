using FleetManagement.Application.Commands.Maintenance;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Queries.FleetStatus;
using FleetManagement.Application.Queries.Maintenance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MaintenanceController(IMediator mediator) : ControllerBase
{
    [HttpGet("vehicle/{vehicleId:int}")]
    public async Task<ActionResult<IReadOnlyList<MaintenanceLogDto>>> GetByVehicle(int vehicleId, CancellationToken ct)
        => Ok(await mediator.Send(new GetMaintenanceByVehicleQuery(vehicleId), ct));

    [HttpPost]
    public async Task<ActionResult<MaintenanceLogDto>> LogMaintenance([FromBody] LogMaintenanceCommand command, CancellationToken ct)
        => Ok(await mediator.Send(command, ct));

    [HttpGet("fuel/vehicle/{vehicleId:int}")]
    public async Task<ActionResult<IReadOnlyList<FuelLogDto>>> GetFuelByVehicle(int vehicleId, CancellationToken ct)
        => Ok(await mediator.Send(new GetFuelLogsByVehicleQuery(vehicleId), ct));

    [HttpPost("fuel")]
    public async Task<ActionResult<FuelLogDto>> LogFuel([FromBody] LogFuelCommand command, CancellationToken ct)
        => Ok(await mediator.Send(command, ct));

    [HttpGet("fleet-status")]
    public async Task<ActionResult<IEnumerable<FleetStatusDto>>> GetFleetStatus([FromQuery] int? warehouseId, CancellationToken ct)
        => Ok(await mediator.Send(new GetFleetStatusQuery(warehouseId), ct));
}
