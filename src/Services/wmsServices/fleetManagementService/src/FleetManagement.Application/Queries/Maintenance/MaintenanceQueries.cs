using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Queries.Maintenance;

public record GetMaintenanceByVehicleQuery(int VehicleId) : IRequest<IReadOnlyList<MaintenanceLogDto>>;
public record GetFuelLogsByVehicleQuery(int VehicleId) : IRequest<IReadOnlyList<FuelLogDto>>;

public class GetMaintenanceByVehicleHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetMaintenanceByVehicleQuery, IReadOnlyList<MaintenanceLogDto>>
{
    public async Task<IReadOnlyList<MaintenanceLogDto>> Handle(GetMaintenanceByVehicleQuery request, CancellationToken ct)
    {
        var logs = await uow.MaintenanceLogs.GetByVehicleAsync(request.VehicleId, ct);
        return logs.Select(mapper.Map<MaintenanceLogDto>).ToList();
    }
}

public class GetFuelLogsByVehicleHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetFuelLogsByVehicleQuery, IReadOnlyList<FuelLogDto>>
{
    public async Task<IReadOnlyList<FuelLogDto>> Handle(GetFuelLogsByVehicleQuery request, CancellationToken ct)
    {
        var logs = await uow.FuelLogs.GetByVehicleAsync(request.VehicleId, ct);
        return logs.Select(mapper.Map<FuelLogDto>).ToList();
    }
}
