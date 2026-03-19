using AutoMapper;
using MediatR;
using MobileAppManagement.Application.DTOs;
using MobileAppManagement.Application.Queries;
using MobileAppManagement.Domain.Interfaces;

namespace MobileAppManagement.Application.Handlers;

public class GetDevicesByEmployeeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetDevicesByEmployeeQuery, IEnumerable<AppDeviceDetailDto>>
{
    public async Task<IEnumerable<AppDeviceDetailDto>> Handle(GetDevicesByEmployeeQuery request, CancellationToken ct)
    {
        var devices = await uow.AppDevices.GetByEmployeeAsync(request.EmployeeSysId, ct);
        return mapper.Map<IEnumerable<AppDeviceDetailDto>>(devices);
    }
}

public class GetDeviceByKeyHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetDeviceByKeyQuery, AppDeviceDetailDto?>
{
    public async Task<AppDeviceDetailDto?> Handle(GetDeviceByKeyQuery request, CancellationToken ct)
    {
        var device = await uow.AppDevices.GetByKeyAsync(request.EmployeeSysId, request.DeviceId, ct);
        return device is null ? null : mapper.Map<AppDeviceDetailDto>(device);
    }
}

public class GetLoginsByUserHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetLoginsByUserQuery, IEnumerable<LoginDetailDto>>
{
    public async Task<IEnumerable<LoginDetailDto>> Handle(GetLoginsByUserQuery request, CancellationToken ct)
    {
        var logins = await uow.LoginDetails.GetByUserAsync(request.UserSysId, ct);
        return mapper.Map<IEnumerable<LoginDetailDto>>(logins);
    }
}

public class GetLoginByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetLoginByIdQuery, LoginDetailDto?>
{
    public async Task<LoginDetailDto?> Handle(GetLoginByIdQuery request, CancellationToken ct)
    {
        var login = await uow.LoginDetails.GetByIdAsync(request.LoginId, ct);
        return login is null ? null : mapper.Map<LoginDetailDto>(login);
    }
}

public class GetRegistrationByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetRegistrationByIdQuery, AppRegistrationDto?>
{
    public async Task<AppRegistrationDto?> Handle(GetRegistrationByIdQuery request, CancellationToken ct)
    {
        var reg = await uow.AppRegistrations.GetByIdAsync(request.RegistrationId, ct);
        return reg is null ? null : mapper.Map<AppRegistrationDto>(reg);
    }
}

public class GetRegistrationsByUserIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetRegistrationsByUserIdQuery, IEnumerable<AppRegistrationDto>>
{
    public async Task<IEnumerable<AppRegistrationDto>> Handle(GetRegistrationsByUserIdQuery request, CancellationToken ct)
    {
        var regs = await uow.AppRegistrations.GetByUserIdAsync(request.UserId, ct);
        return mapper.Map<IEnumerable<AppRegistrationDto>>(regs);
    }
}

public class GetRegistrationsByStatusHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetRegistrationsByStatusQuery, IEnumerable<AppRegistrationDto>>
{
    public async Task<IEnumerable<AppRegistrationDto>> Handle(GetRegistrationsByStatusQuery request, CancellationToken ct)
    {
        var regs = await uow.AppRegistrations.GetByStatusAsync(request.Status, ct);
        return mapper.Map<IEnumerable<AppRegistrationDto>>(regs);
    }
}
