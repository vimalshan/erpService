using MediatR;
using FluentValidation;
using MobileAppManagement.Application.Commands;
using MobileAppManagement.Application.DTOs;

namespace MobileAppManagement.API.GraphQL;

/// <summary>
/// GraphQL mutations for device, login, and registration operations
/// </summary>
public class Mutation
{
    /// <summary>
    /// Register a device for an employee
    /// </summary>
    public async Task<string> RegisterDevice(
        [Service] IMediator mediator,
        [Service] ILogger<Mutation> logger,
        decimal employeeSysId, string deviceId, string deviceType, string? imeiNo, decimal updatedBy,
        CancellationToken ct)
    {
        try
        {
            // Validate input
            if (employeeSysId <= 0)
                throw new ArgumentException("EmployeeSysId must be greater than 0", nameof(employeeSysId));
            if (string.IsNullOrWhiteSpace(deviceId))
                throw new ArgumentException("DeviceId is required", nameof(deviceId));
            if (string.IsNullOrEmpty(deviceType) || !new[] { "A", "I", "a", "i" }.Contains(deviceType))
                throw new ArgumentException("DeviceType must be 'A' (Android) or 'I' (iOS)", nameof(deviceType));
            if (updatedBy <= 0)
                throw new ArgumentException("UpdatedBy must be greater than 0", nameof(updatedBy));

            var result = await mediator.Send(
                new RegisterDeviceCommand(employeeSysId, deviceId, deviceType, imeiNo, updatedBy), 
                ct);
            
            return result;
        }
        catch (ValidationException ex)
        {
            logger.LogWarning("RegisterDevice validation failed: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));
            throw new GraphQLException($"Validation failed: {string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "RegisterDevice error for employee {EmployeeSysId}", employeeSysId);
            throw new GraphQLException("Device registration failed. Please check your input and try again.");
        }
    }

    /// <summary>
    /// Deactivate a device
    /// </summary>
    public async Task<string> DeactivateDevice(
        [Service] IMediator mediator,
        [Service] ILogger<Mutation> logger,
        decimal employeeSysId, string deviceId, decimal updatedBy,
        CancellationToken ct)
    {
        try
        {
            if (employeeSysId <= 0)
                throw new ArgumentException("EmployeeSysId must be greater than 0", nameof(employeeSysId));
            if (string.IsNullOrWhiteSpace(deviceId))
                throw new ArgumentException("DeviceId is required", nameof(deviceId));
            if (updatedBy <= 0)
                throw new ArgumentException("UpdatedBy must be greater than 0", nameof(updatedBy));

            return await mediator.Send(new DeactivateDeviceCommand(employeeSysId, deviceId, updatedBy), ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "DeactivateDevice error for device {DeviceId}", deviceId);
            throw new GraphQLException("Device deactivation failed. Please try again.");
        }
    }

    /// <summary>
    /// Log a user login
    /// </summary>
    public async Task<decimal> LogUserLogin(
        [Service] IMediator mediator,
        [Service] ILogger<Mutation> logger,
        decimal userSysId, string? deviceId, string? imeiNo, string? deviceType,
        CancellationToken ct)
    {
        try
        {
            if (userSysId <= 0)
                throw new ArgumentException("UserSysId must be greater than 0", nameof(userSysId));
            if (!string.IsNullOrEmpty(deviceType) && !new[] { "A", "I", "a", "i" }.Contains(deviceType))
                throw new ArgumentException("DeviceType must be 'A' (Android) or 'I' (iOS)", nameof(deviceType));

            return await mediator.Send(new LogUserLoginCommand(userSysId, deviceId, imeiNo, deviceType), ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "LogUserLogin error for user {UserSysId}", userSysId);
            throw new GraphQLException("Login logging failed. Please try again.");
        }
    }

    /// <summary>
    /// Create a new registration
    /// </summary>
    public async Task<AppRegistrationDto> CreateRegistration(
        [Service] IMediator mediator,
        [Service] ILogger<Mutation> logger,
        long registrationId, long? employeeSysId, string? userId, long? userSysId,
        string? userType, string? mobileNo, string? imeiNo, string? deviceId, string? deviceType,
        CancellationToken ct)
    {
        try
        {
            if (userSysId.HasValue && userSysId <= 0)
                throw new ArgumentException("UserSysId must be greater than 0", nameof(userSysId));
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId is required", nameof(userId));
            if (employeeSysId.HasValue && employeeSysId <= 0)
                throw new ArgumentException("EmployeeSysId must be greater than 0", nameof(employeeSysId));

            return await mediator.Send(new CreateRegistrationCommand(registrationId, employeeSysId,
                userId, userSysId, userType, mobileNo, imeiNo, deviceId, deviceType), ct);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning("CreateRegistration validation failed: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));
            throw new GraphQLException($"Validation failed: {string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateRegistration error for user {UserId}", userId);
            throw new GraphQLException("Registration creation failed. Please try again.");
        }
    }

    /// <summary>
    /// Update registration status
    /// </summary>
    public async Task<string> UpdateRegistrationStatus(
        [Service] IMediator mediator,
        [Service] ILogger<Mutation> logger,
        long registrationId, string newStatus,
        CancellationToken ct)
    {
        try
        {
            if (registrationId <= 0)
                throw new ArgumentException("RegistrationId must be greater than 0", nameof(registrationId));
            if (string.IsNullOrEmpty(newStatus) || newStatus.Length != 1 || !char.IsLetter(newStatus[0]))
                throw new ArgumentException("NewStatus must be a single letter", nameof(newStatus));

            return await mediator.Send(new UpdateRegistrationStatusCommand(registrationId, newStatus), ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateRegistrationStatus error for registration {RegistrationId}", registrationId);
            throw new GraphQLException("Status update failed. Please try again.");
        }
    }

    /// <summary>
    /// Generate registration PIN
    /// </summary>
    public async Task<long> GenerateRegistrationPin(
        [Service] IMediator mediator,
        [Service] ILogger<Mutation> logger,
        long registrationId,
        CancellationToken ct)
    {
        try
        {
            if (registrationId <= 0)
                throw new ArgumentException("RegistrationId must be greater than 0", nameof(registrationId));

            return await mediator.Send(new GenerateRegistrationPinCommand(registrationId), ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GenerateRegistrationPin error for registration {RegistrationId}", registrationId);
            throw new GraphQLException("PIN generation failed. Please try again.");
        }
    }
}

/// <summary>
/// Custom GraphQL exception for better error reporting
/// </summary>
public class GraphQLException : Exception
{
    public GraphQLException(string message) : base(message) { }
}
