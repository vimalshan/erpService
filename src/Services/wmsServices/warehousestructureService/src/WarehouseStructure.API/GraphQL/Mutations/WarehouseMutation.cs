using MediatR;
using WarehouseStructure.Application.Commands.CreateWarehouse;
using WarehouseStructure.Application.Commands.CreateZone;
using WarehouseStructure.Application.Commands.DeleteWarehouse;
using WarehouseStructure.Application.Commands.DeleteZone;
using WarehouseStructure.Application.Commands.UpdateWarehouse;
using WarehouseStructure.Application.Commands.UpdateZone;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.API.GraphQL.Mutations;

public class WarehouseMutation
{
    public async Task<WarehouseDto> CreateWarehouse([Service] IMediator mediator, CreateWarehouseDto input, CancellationToken ct)
    {
        return await mediator.Send(new CreateWarehouseCommand(input), ct);
    }

    public async Task<WarehouseDto> UpdateWarehouse([Service] IMediator mediator, int id, UpdateWarehouseDto input, CancellationToken ct)
    {
        return await mediator.Send(new UpdateWarehouseCommand(id, input), ct);
    }

    public async Task<bool> DeleteWarehouse([Service] IMediator mediator, int id, CancellationToken ct)
    {
        return await mediator.Send(new DeleteWarehouseCommand(id), ct);
    }

    public async Task<ZoneDto> CreateZone([Service] IMediator mediator, CreateZoneDto input, CancellationToken ct)
    {
        return await mediator.Send(new CreateZoneCommand(input), ct);
    }

    public async Task<ZoneDto> UpdateZone([Service] IMediator mediator, int id, UpdateZoneDto input, CancellationToken ct)
    {
        return await mediator.Send(new UpdateZoneCommand(id, input), ct);
    }

    public async Task<bool> DeleteZone([Service] IMediator mediator, int id, CancellationToken ct)
    {
        return await mediator.Send(new DeleteZoneCommand(id), ct);
    }
}
