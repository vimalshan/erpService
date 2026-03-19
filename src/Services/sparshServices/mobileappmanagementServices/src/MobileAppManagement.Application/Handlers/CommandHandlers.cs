using AutoMapper;
using MediatR;
using MobileAppManagement.Application.Commands;
using MobileAppManagement.Application.DTOs;
using MobileAppManagement.Domain.Entities;
using MobileAppManagement.Domain.Interfaces;

namespace MobileAppManagement.Application.Handlers;

public class RegisterDeviceHandler(IUnitOfWork uow) : IRequestHandler<RegisterDeviceCommand, string>
{
    public async Task<string> Handle(RegisterDeviceCommand request, CancellationToken ct)
    {
        var existing = await uow.AppDevices.GetByKeyAsync(request.EmployeeSysId, request.DeviceId, ct);
        if (existing is not null)
        {
            existing.UpdateDevice(request.DeviceType, request.ImeiNo, request.UpdatedBy);
            await uow.AppDevices.UpdateAsync(existing, ct);
        }
        else
        {
            var device = AppDeviceDetail.Create(request.EmployeeSysId, request.DeviceId,
                request.DeviceType, request.ImeiNo, request.UpdatedBy);
            await uow.AppDevices.AddAsync(device, ct);
        }
        await uow.SaveChangesAsync(ct);
        return "Device registered successfully.";
    }
}

public class DeactivateDeviceHandler(IUnitOfWork uow) : IRequestHandler<DeactivateDeviceCommand, string>
{
    public async Task<string> Handle(DeactivateDeviceCommand request, CancellationToken ct)
    {
        var device = await uow.AppDevices.GetByKeyAsync(request.EmployeeSysId, request.DeviceId, ct)
            ?? throw new KeyNotFoundException($"Device {request.DeviceId} not found for employee {request.EmployeeSysId}.");
        device.Deactivate(request.UpdatedBy);
        await uow.AppDevices.UpdateAsync(device, ct);
        await uow.SaveChangesAsync(ct);
        return "Device deactivated successfully.";
    }
}

public class LogUserLoginHandler(IUnitOfWork uow) : IRequestHandler<LogUserLoginCommand, decimal>
{
    public async Task<decimal> Handle(LogUserLoginCommand request, CancellationToken ct)
    {
        // Use Guid converted to long for uniqueness, or better: let DB generate with sequence
        var loginId = (long)Math.Abs(System.Guid.NewGuid().GetHashCode());
        var login = LoginDetail.Create(loginId, request.UserSysId, request.DeviceId,
            request.ImeiNo, request.DeviceType);
        await uow.LoginDetails.AddAsync(login, ct);
        await uow.SaveChangesAsync(ct);
        return login.LoginId;
    }
}

public class CreateRegistrationHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateRegistrationCommand, AppRegistrationDto>
{
    public async Task<AppRegistrationDto> Handle(CreateRegistrationCommand request, CancellationToken ct)
    {
        var reg = AppRegistration.Create(request.RegistrationId, request.EmployeeSysId,
            request.UserId, request.UserSysId, request.UserType, request.MobileNo,
            request.ImeiNo, request.DeviceId, request.DeviceType);
        await uow.AppRegistrations.AddAsync(reg, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<AppRegistrationDto>(reg);
    }
}

public class UpdateRegistrationStatusHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateRegistrationStatusCommand, string>
{
    public async Task<string> Handle(UpdateRegistrationStatusCommand request, CancellationToken ct)
    {
        var reg = await uow.AppRegistrations.GetByIdAsync(request.RegistrationId, ct)
            ?? throw new KeyNotFoundException($"Registration {request.RegistrationId} not found.");
        reg.ChangeStatus(request.NewStatus);
        await uow.AppRegistrations.UpdateAsync(reg, ct);
        await uow.SaveChangesAsync(ct);
        return "Registration status updated successfully.";
    }
}

public class GenerateRegistrationPinHandler(IUnitOfWork uow)
    : IRequestHandler<GenerateRegistrationPinCommand, long>
{
    public async Task<long> Handle(GenerateRegistrationPinCommand request, CancellationToken ct)
    {
        var reg = await uow.AppRegistrations.GetByIdAsync(request.RegistrationId, ct)
            ?? throw new KeyNotFoundException($"Registration {request.RegistrationId} not found.");
        // Use Random.Shared for thread-safe generation, upper bound inclusive with 1000000
        var pin = Random.Shared.Next(100000, 1000000);
        reg.GeneratePin(pin);
        // Don't call UpdateAsync - entity is already tracked after GetByIdAsync
        await uow.SaveChangesAsync(ct);
        return pin;
    }
}
