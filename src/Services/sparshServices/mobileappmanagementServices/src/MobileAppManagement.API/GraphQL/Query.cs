using MediatR;
using MobileAppManagement.Application.DTOs;
using MobileAppManagement.Application.Queries;

namespace MobileAppManagement.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<AppDeviceDetailDto>> GetDevicesByEmployee(
        [Service] IMediator mediator, decimal employeeSysId, CancellationToken ct)
        => await mediator.Send(new GetDevicesByEmployeeQuery(employeeSysId), ct);

    public async Task<AppDeviceDetailDto?> GetDevice(
        [Service] IMediator mediator, decimal employeeSysId, string deviceId, CancellationToken ct)
        => await mediator.Send(new GetDeviceByKeyQuery(employeeSysId, deviceId), ct);

    public async Task<IEnumerable<LoginDetailDto>> GetLoginsByUser(
        [Service] IMediator mediator, decimal userSysId, CancellationToken ct)
        => await mediator.Send(new GetLoginsByUserQuery(userSysId), ct);

    public async Task<LoginDetailDto?> GetLogin(
        [Service] IMediator mediator, decimal loginId, CancellationToken ct)
        => await mediator.Send(new GetLoginByIdQuery(loginId), ct);

    public async Task<AppRegistrationDto?> GetRegistration(
        [Service] IMediator mediator, long registrationId, CancellationToken ct)
        => await mediator.Send(new GetRegistrationByIdQuery(registrationId), ct);

    public async Task<IEnumerable<AppRegistrationDto>> GetRegistrationsByUserId(
        [Service] IMediator mediator, string userId, CancellationToken ct)
        => await mediator.Send(new GetRegistrationsByUserIdQuery(userId), ct);

    public async Task<IEnumerable<AppRegistrationDto>> GetRegistrationsByStatus(
        [Service] IMediator mediator, string status, CancellationToken ct)
        => await mediator.Send(new GetRegistrationsByStatusQuery(status), ct);
}
