namespace MobileAppManagement.Application.DTOs;

public record AppDeviceDetailDto(
    decimal EmployeeSysId,
    string DeviceId,
    string Active,
    string? DeviceType,
    string? ImeiNo,
    DateTime CreatedOn,
    DateTime UpdatedOn);

public record LoginDetailDto(
    decimal LoginId,
    decimal UserSysId,
    string? DeviceId,
    DateTime Logon,
    string Guid,
    string? ImeiNo,
    string? DeviceType);

public record AppRegistrationDto(
    long RegistrationId,
    long? EmployeeSysId,
    string? UserId,
    long? UserSysId,
    string? UserType,
    long? PinNo,
    DateTime? PinGeneratedOn,
    DateTime? UpdatedOn,
    string? Status,
    string? MobileNo,
    string? ImeiNo,
    string? DeviceId,
    string? DeviceType);
