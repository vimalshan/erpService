namespace FleetManagement.Application.DTOs;

public record VehicleDto(
    int VehicleId, string Code, string LicensePlate, string VehicleType,
    string? Make, string? Model, int? Year,
    decimal? CapacityWeight, decimal? CapacityVolume,
    string Status, int? WarehouseId, string? Notes, bool IsActive,
    DateTime CreatedDate, DateTime ModifiedDate);

public record DriverDto(
    int DriverId, string Code, int? EmployeeId, string FullName,
    string LicenseNumber, DateTime LicenseExpiry,
    string? Phone, string? Email, bool IsActive,
    DateTime CreatedDate, DateTime ModifiedDate);

public record RouteDto(
    int RouteId, string RouteName, string? Description,
    string? StartLocation, string? EndLocation,
    int? EstimatedDuration, bool IsActive, DateTime CreatedDate);

public record TripDto(
    int TripId, string TripNumber, int? RouteId, int VehicleId, int DriverId,
    DateTime TripDate, DateTime? StartTime, DateTime? EndTime,
    string? OriginType, int? OriginId, string? DestinationType, int? DestinationId,
    string Status, string? Notes, string? CreatedBy,
    DateTime CreatedDate, DateTime ModifiedDate,
    List<TripStopDto>? Stops);

public record TripStopDto(
    int StopId, int TripId, int StopSequence, string? StopType,
    string? LocationType, int? LocationId, string? Address,
    DateTime? PlannedArrival, DateTime? ActualArrival,
    DateTime? PlannedDeparture, DateTime? ActualDeparture,
    string Status, string? Notes);

public record MaintenanceLogDto(
    int LogId, int VehicleId, DateTime MaintenanceDate, string MaintenanceType,
    string? Description, decimal? Cost, int? OdometerReading,
    DateTime? NextDueDate, string? PerformedBy, DateTime CreatedDate);

public record FuelLogDto(
    int FuelLogId, int VehicleId, DateTime FuelDate,
    decimal? Gallons, decimal? Cost, int? OdometerReading, string? Notes);

public record FleetStatusDto(
    int VehicleId, string Code, string LicensePlate, string VehicleType,
    string Status, string? HomeWarehouse, int ActiveTrips,
    DateTime? LastMaintenance, DateTime? NextMaintenanceDue);
