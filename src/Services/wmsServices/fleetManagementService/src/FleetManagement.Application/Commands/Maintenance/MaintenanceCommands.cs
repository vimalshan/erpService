using FleetManagement.Application.DTOs;
using MediatR;

namespace FleetManagement.Application.Commands.Maintenance;

public record LogMaintenanceCommand(
    int VehicleId, DateTime MaintenanceDate, string MaintenanceType,
    string? Description, decimal? Cost, int? OdometerReading,
    DateTime? NextDueDate, string? PerformedBy) : IRequest<MaintenanceLogDto>;

public record LogFuelCommand(
    int VehicleId, decimal Gallons, decimal Cost,
    int? OdometerReading, string? Notes) : IRequest<FuelLogDto>;
