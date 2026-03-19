using MediatR;
using MobileAppManagement.Application.DTOs;

namespace MobileAppManagement.Application.Commands;

// Register Device
public record RegisterDeviceCommand(
    decimal EmployeeSysId,
    string DeviceId,
    string DeviceType,
    string? ImeiNo,
    decimal UpdatedBy) : IRequest<string>;

// Deactivate Device
public record DeactivateDeviceCommand(
    decimal EmployeeSysId,
    string DeviceId,
    decimal UpdatedBy) : IRequest<string>;

// Log User Login
public record LogUserLoginCommand(
    decimal UserSysId,
    string? DeviceId,
    string? ImeiNo,
    string? DeviceType) : IRequest<decimal>;

// Create Registration
public record CreateRegistrationCommand(
    long RegistrationId,
    long? EmployeeSysId,
    string? UserId,
    long? UserSysId,
    string? UserType,
    string? MobileNo,
    string? ImeiNo,
    string? DeviceId,
    string? DeviceType) : IRequest<AppRegistrationDto>;

// Update Registration Status
public record UpdateRegistrationStatusCommand(
    long RegistrationId,
    string NewStatus) : IRequest<string>;

// Generate Registration PIN
public record GenerateRegistrationPinCommand(
    long RegistrationId) : IRequest<long>;
