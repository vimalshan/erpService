using MediatR;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.Roles.Commands;
using OrganizationSetup.Application.OrgParams.Commands;
using OrganizationSetup.Application.UserMaps.Commands;
using OrganizationSetup.Application.PpLimits.Commands;

namespace OrganizationSetup.API.GraphQL;

[MutationType]
public class OrganizationSetupMutation
{
    // ── Roles ────────────────────────────────────────────────────────────

    public async Task<RoleDto> CreateRole([Service] IMediator mediator, CreateRoleInput input, CancellationToken ct)
        => await mediator.Send(new CreateRoleCommand(input.RoleId, input.RoleName, input.RoleLevel), ct);

    // ── OrgParams ────────────────────────────────────────────────────────

    public async Task<OrgParamsDto> CreateOrgParam([Service] IMediator mediator, CreateOrgParamInput input, CancellationToken ct)
        => await mediator.Send(new CreateOrgParamCommand(input.ParamId, input.ParamType, input.ParamValue, input.OrgId), ct);

    public async Task<OrgParamsDto> UpdateOrgParam([Service] IMediator mediator, long paramId, long newValue, CancellationToken ct)
        => await mediator.Send(new UpdateOrgParamCommand(paramId, newValue), ct);

    // ── UserMaps ─────────────────────────────────────────────────────────

    public async Task<UserMapDto> CreateUserMap([Service] IMediator mediator, CreateUserMapInput input, CancellationToken ct)
        => await mediator.Send(new CreateUserMapCommand(input.MapId, input.RoleId, input.EmpSysId, input.OrgId, input.Business), ct);

    public async Task<bool> DeleteUserMap([Service] IMediator mediator, long mapId, CancellationToken ct)
        => await mediator.Send(new DeleteUserMapCommand(mapId), ct);

    // ── PpLimits ─────────────────────────────────────────────────────────

    public async Task<PpLimitDto> CreatePpLimit([Service] IMediator mediator, CreatePpLimitInput input, CancellationToken ct)
        => await mediator.Send(new CreatePpLimitCommand(input.LimitId, input.OrgId, input.TranType, input.BaseCurr, input.LimitAmt, input.FinYear), ct);

    public async Task<PpLimitDto> UpdatePpLimit([Service] IMediator mediator, long limitId, decimal? newLimitAmt, decimal? newLimitAct, CancellationToken ct)
        => await mediator.Send(new UpdatePpLimitCommand(limitId, newLimitAmt, newLimitAct), ct);
}

// ── Input Types ──────────────────────────────────────────────────────────────

public record CreateRoleInput(long RoleId, string RoleName, long RoleLevel);

public record CreateOrgParamInput(long ParamId, string ParamType, long ParamValue, long OrgId);

public record CreateUserMapInput(long MapId, long RoleId, long EmpSysId, long OrgId, long? Business = null);

public record CreatePpLimitInput(long LimitId, long OrgId, string TranType, long BaseCurr, decimal? LimitAmt, int FinYear);
