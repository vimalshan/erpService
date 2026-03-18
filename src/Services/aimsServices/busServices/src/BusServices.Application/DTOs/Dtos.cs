namespace BusServices.Application.DTOs;

public record BusDto(
    int BusId,
    string RegistrationNumber,
    string? Description,
    int Capacity,
    int? CapacityReserved,
    DateTime OperatingFrom,
    long LastModifiedBy,
    DateTime LastModifiedOn);

public record BusRouteDto(
    int RouteId,
    int BusId,
    string Name,
    string? Description,
    string Status,
    long LastModifiedBy,
    DateTime LastModifiedOn);

public record EmployeeBusDto(
    long EmpBusId,
    long EmpSysId,
    int BusId,
    int RouteId,
    DateTime EffectiveDate,
    DateTime? ClosingDate,
    long LastModifiedBy,
    DateTime LastModifiedOn);

public record BusArrivalDto(
    long ArrivalId,
    int BusId,
    DateTime ArrivalDate,
    string ArrivalTime,
    string Status,
    string? Remarks,
    long LastModifiedBy,
    DateTime LastModifiedOn);

public record BusDeductionRateDto(
    int DeductId,
    int BusId,
    decimal Amount,
    DateTime EffectiveDate,
    DateTime? ClosingDate,
    long LastModifiedBy,
    DateTime LastModifiedOn);
