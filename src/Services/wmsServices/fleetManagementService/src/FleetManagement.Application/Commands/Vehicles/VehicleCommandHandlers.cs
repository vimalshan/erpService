using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Enums;
using FleetManagement.Domain.Events;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Commands.Vehicles;

public class CreateVehicleHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    public async Task<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken ct)
    {
        var vehicle = new Vehicle
        {
            Code = request.Code,
            LicensePlate = request.LicensePlate,
            VehicleType = Enum.Parse<VehicleType>(request.VehicleType),
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            CapacityWeight = request.CapacityWeight,
            CapacityVolume = request.CapacityVolume,
            WarehouseId = request.WarehouseId,
            Notes = request.Notes
        };
        await uow.Vehicles.AddAsync(vehicle, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<VehicleDto>(vehicle);
    }
}

public class UpdateVehicleHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateVehicleCommand, VehicleDto>
{
    public async Task<VehicleDto> Handle(UpdateVehicleCommand request, CancellationToken ct)
    {
        var vehicle = await uow.Vehicles.GetByIdAsync(request.VehicleId, ct)
            ?? throw new KeyNotFoundException($"Vehicle {request.VehicleId} not found.");
        vehicle.LicensePlate = request.LicensePlate;
        vehicle.Make = request.Make;
        vehicle.Model = request.Model;
        vehicle.Year = request.Year;
        vehicle.CapacityWeight = request.CapacityWeight;
        vehicle.CapacityVolume = request.CapacityVolume;
        vehicle.WarehouseId = request.WarehouseId;
        vehicle.Notes = request.Notes;
        vehicle.ModifiedDate = DateTime.UtcNow;
        await uow.Vehicles.UpdateAsync(vehicle, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<VehicleDto>(vehicle);
    }
}

public class ChangeVehicleStatusHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<ChangeVehicleStatusCommand, VehicleDto>
{
    public async Task<VehicleDto> Handle(ChangeVehicleStatusCommand request, CancellationToken ct)
    {
        var vehicle = await uow.Vehicles.GetByIdAsync(request.VehicleId, ct)
            ?? throw new KeyNotFoundException($"Vehicle {request.VehicleId} not found.");
        var status = Enum.Parse<VehicleStatus>(request.Status);
        vehicle.Status = status;
        vehicle.ModifiedDate = DateTime.UtcNow;
        vehicle.AddDomainEvent(new VehicleStatusChangedEvent(vehicle.VehicleId, status));
        await uow.Vehicles.UpdateAsync(vehicle, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<VehicleDto>(vehicle);
    }
}

public class DeleteVehicleHandler(IUnitOfWork uow) : IRequestHandler<DeleteVehicleCommand, bool>
{
    public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken ct)
    {
        var vehicle = await uow.Vehicles.GetByIdAsync(request.VehicleId, ct);
        if (vehicle is null) return false;
        await uow.Vehicles.DeleteAsync(vehicle, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
