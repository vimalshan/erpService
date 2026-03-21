using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Events;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Commands.Maintenance;

public class LogMaintenanceHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<LogMaintenanceCommand, MaintenanceLogDto>
{
    public async Task<MaintenanceLogDto> Handle(LogMaintenanceCommand request, CancellationToken ct)
    {
        var log = new MaintenanceLog
        {
            VehicleId = request.VehicleId,
            MaintenanceDate = request.MaintenanceDate,
            MaintenanceType = request.MaintenanceType,
            Description = request.Description,
            Cost = request.Cost,
            OdometerReading = request.OdometerReading,
            NextDueDate = request.NextDueDate,
            PerformedBy = request.PerformedBy
        };
        await uow.MaintenanceLogs.AddAsync(log, ct);

        var vehicle = await uow.Vehicles.GetByIdAsync(request.VehicleId, ct);
        if (vehicle is not null)
            vehicle.SetMaintenance();

        log.AddDomainEvent(new MaintenanceLoggedEvent(log.LogId, log.VehicleId, log.MaintenanceType));
        await uow.SaveChangesAsync(ct);
        return mapper.Map<MaintenanceLogDto>(log);
    }
}

public class LogFuelHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<LogFuelCommand, FuelLogDto>
{
    public async Task<FuelLogDto> Handle(LogFuelCommand request, CancellationToken ct)
    {
        var log = new FuelLog
        {
            VehicleId = request.VehicleId,
            Gallons = request.Gallons,
            Cost = request.Cost,
            OdometerReading = request.OdometerReading,
            Notes = request.Notes
        };
        log.AddDomainEvent(new FuelLoggedEvent(log.FuelLogId, log.VehicleId, log.Gallons, log.Cost));
        await uow.FuelLogs.AddAsync(log, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<FuelLogDto>(log);
    }
}
