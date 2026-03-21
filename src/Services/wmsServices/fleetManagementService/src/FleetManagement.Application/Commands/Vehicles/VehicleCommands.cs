using FleetManagement.Application.DTOs;
using MediatR;

namespace FleetManagement.Application.Commands.Vehicles;

public record CreateVehicleCommand(
    string Code, string LicensePlate, string VehicleType,
    string? Make, string? Model, int? Year,
    decimal? CapacityWeight, decimal? CapacityVolume,
    int? WarehouseId, string? Notes) : IRequest<VehicleDto>;

public record UpdateVehicleCommand(
    int VehicleId, string LicensePlate, string? Make, string? Model,
    int? Year, decimal? CapacityWeight, decimal? CapacityVolume,
    int? WarehouseId, string? Notes) : IRequest<VehicleDto>;

public record ChangeVehicleStatusCommand(int VehicleId, string Status) : IRequest<VehicleDto>;

public record DeleteVehicleCommand(int VehicleId) : IRequest<bool>;
