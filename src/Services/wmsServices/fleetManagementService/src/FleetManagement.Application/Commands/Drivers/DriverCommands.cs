using FleetManagement.Application.DTOs;
using MediatR;

namespace FleetManagement.Application.Commands.Drivers;

public record CreateDriverCommand(
    string Code, int? EmployeeId, string FullName,
    string LicenseNumber, DateTime LicenseExpiry,
    string? Phone, string? Email) : IRequest<DriverDto>;

public record UpdateDriverCommand(
    int DriverId, string FullName,
    string LicenseNumber, DateTime LicenseExpiry,
    string? Phone, string? Email) : IRequest<DriverDto>;

public record DeleteDriverCommand(int DriverId) : IRequest<bool>;
