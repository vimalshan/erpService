using MediatR;
using AdminService.Application.Commands.AdminMasters;
using AdminService.Application.Commands.UserMaps;
using AdminService.Application.Commands.FinUserMaps;
using AdminService.Application.Commands.AccessRights;
using AdminService.Application.DTOs;

namespace AdminService.API.GraphQL;

public class AdminMutation
{
    public async Task<AdminMasterDto> CreateAdminMaster(
        CreateAdminMasterCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<AdminMasterDto> UpdateAdminMaster(
        UpdateAdminMasterCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteAdminMaster(
        string adminId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteAdminMasterCommand(adminId), ct);

    public async Task<AdminUserMapDto> CreateAdminUserMap(
        CreateAdminUserMapCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<AdminUserMapDto> UpdateAdminUserMap(
        UpdateAdminUserMapCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteAdminUserMap(
        string mapId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteAdminUserMapCommand(mapId), ct);

    public async Task<AdminFinUserMapDto> CreateAdminFinUserMap(
        CreateAdminFinUserMapCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<AdminAccessRightsDto> CreateAccessRights(
        CreateAccessRightsCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<AdminAccessRightsDto> UpdateAccessRights(
        UpdateAccessRightsCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteAccessRights(
        string rightsId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteAccessRightsCommand(rightsId), ct);
}
