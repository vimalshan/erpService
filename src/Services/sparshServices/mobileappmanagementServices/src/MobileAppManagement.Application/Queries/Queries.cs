using MediatR;
using MobileAppManagement.Application.DTOs;

namespace MobileAppManagement.Application.Queries;

public record GetDevicesByEmployeeQuery(decimal EmployeeSysId) : IRequest<IEnumerable<AppDeviceDetailDto>>;

public record GetDeviceByKeyQuery(decimal EmployeeSysId, string DeviceId) : IRequest<AppDeviceDetailDto?>;

public record GetLoginsByUserQuery(decimal UserSysId) : IRequest<IEnumerable<LoginDetailDto>>;

public record GetLoginByIdQuery(decimal LoginId) : IRequest<LoginDetailDto?>;

public record GetRegistrationByIdQuery(long RegistrationId) : IRequest<AppRegistrationDto?>;

public record GetRegistrationsByUserIdQuery(string UserId) : IRequest<IEnumerable<AppRegistrationDto>>;

public record GetRegistrationsByStatusQuery(string Status) : IRequest<IEnumerable<AppRegistrationDto>>;
