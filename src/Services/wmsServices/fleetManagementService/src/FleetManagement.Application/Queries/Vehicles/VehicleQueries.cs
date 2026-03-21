using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Enums;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Queries.Vehicles;

public record GetVehicleByIdQuery(int VehicleId) : IRequest<VehicleDto?>;
public record GetAllVehiclesQuery : IRequest<IReadOnlyList<VehicleDto>>;
public record GetVehiclesByStatusQuery(string Status) : IRequest<IReadOnlyList<VehicleDto>>;

public class GetVehicleByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
{
    public async Task<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken ct)
    {
        var vehicle = await uow.Vehicles.GetByIdAsync(request.VehicleId, ct);
        return vehicle is null ? null : mapper.Map<VehicleDto>(vehicle);
    }
}

public class GetAllVehiclesHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllVehiclesQuery, IReadOnlyList<VehicleDto>>
{
    public async Task<IReadOnlyList<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken ct)
    {
        var vehicles = await uow.Vehicles.GetAllAsync(ct);
        return vehicles.Select(mapper.Map<VehicleDto>).ToList();
    }
}

public class GetVehiclesByStatusHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetVehiclesByStatusQuery, IReadOnlyList<VehicleDto>>
{
    public async Task<IReadOnlyList<VehicleDto>> Handle(GetVehiclesByStatusQuery request, CancellationToken ct)
    {
        var status = Enum.Parse<VehicleStatus>(request.Status);
        var vehicles = await uow.Vehicles.GetByStatusAsync(status, ct);
        return vehicles.Select(mapper.Map<VehicleDto>).ToList();
    }
}
