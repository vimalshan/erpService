using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Application.Queries.GetAllWarehouses;
using WarehouseStructure.Application.Queries.GetAllZones;
using WarehouseStructure.Application.Queries.GetWarehouseById;
using WarehouseStructure.Application.Queries.GetZoneById;

namespace WarehouseStructure.API.GraphQL.Queries;

public class WarehouseQuery
{
    public async Task<IEnumerable<WarehouseDto>> GetWarehouses([Service] IMediator mediator, CancellationToken ct)
    {
        return await mediator.Send(new GetAllWarehousesQuery(), ct);
    }

    public async Task<WarehouseDto?> GetWarehouseById([Service] IMediator mediator, int id, CancellationToken ct)
    {
        return await mediator.Send(new GetWarehouseByIdQuery(id), ct);
    }

    public async Task<IEnumerable<ZoneDto>> GetZones([Service] IMediator mediator, int? warehouseId, CancellationToken ct)
    {
        return await mediator.Send(new GetAllZonesQuery(warehouseId), ct);
    }

    public async Task<ZoneDto?> GetZoneById([Service] IMediator mediator, int id, CancellationToken ct)
    {
        return await mediator.Send(new GetZoneByIdQuery(id), ct);
    }
}
