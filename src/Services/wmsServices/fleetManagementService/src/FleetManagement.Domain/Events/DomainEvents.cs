using FleetManagement.Domain.Enums;
using MediatR;

namespace FleetManagement.Domain.Events;

public record VehicleStatusChangedEvent(int VehicleId, VehicleStatus NewStatus) : INotification;

public record TripStatusChangedEvent(int TripId, TripStatus NewStatus) : INotification;

public record TripCreatedEvent(int TripId, string TripNumber, int VehicleId, int DriverId) : INotification;

public record MaintenanceLoggedEvent(int LogId, int VehicleId, string MaintenanceType) : INotification;

public record FuelLoggedEvent(int FuelLogId, int VehicleId, decimal? Gallons, decimal? Cost) : INotification;

public record DriverLicenseExpiringEvent(int DriverId, string FullName, DateTime LicenseExpiry) : INotification;
