using MediatR;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.Roles.Queries;
using OrganizationSetup.Application.OrgParams.Queries;
using OrganizationSetup.Application.UserMaps.Queries;
using OrganizationSetup.Application.PpLimits.Queries;

namespace OrganizationSetup.API.GraphQL;

[QueryType]
public class OrganizationSetupQuery
{
    // ── Roles ────────────────────────────────────────────────────────────

    public async Task<IEnumerable<RoleDto>> GetRoles(IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetRolesQuery(), ct);

    public async Task<RoleDto?> GetRole(IMediator mediator, long roleId, CancellationToken ct)
        => await mediator.Send(new GetRoleByIdQuery(roleId), ct);

    // ── OrgParams ────────────────────────────────────────────────────────

    public async Task<IEnumerable<OrgParamsDto>> GetOrgParams(IMediator mediator, long orgId, CancellationToken ct)
        => await mediator.Send(new GetOrgParamsByOrgQuery(orgId), ct);

    public async Task<OrgParamsDto?> GetOrgParamByType(IMediator mediator, long orgId, string paramType, CancellationToken ct)
        => await mediator.Send(new GetOrgParamByTypeQuery(orgId, paramType), ct);

    // ── UserMaps ─────────────────────────────────────────────────────────

    public async Task<IEnumerable<UserMapDto>> GetUserMapsByOrg(IMediator mediator, long orgId, CancellationToken ct)
        => await mediator.Send(new GetUserMapsByOrgQuery(orgId), ct);

    public async Task<IEnumerable<UserMapDto>> GetUserMapsByEmployee(IMediator mediator, long empSysId, CancellationToken ct)
        => await mediator.Send(new GetUserMapsByEmployeeQuery(empSysId), ct);

    // ── PpLimits ─────────────────────────────────────────────────────────

    public async Task<IEnumerable<PpLimitDto>> GetPpLimits(IMediator mediator, long orgId, int finYear, CancellationToken ct)
        => await mediator.Send(new GetPpLimitsByOrgAndYearQuery(orgId, finYear), ct);

    public async Task<PpLimitDto?> GetPpLimit(IMediator mediator, long limitId, CancellationToken ct)
        => await mediator.Send(new GetPpLimitByIdQuery(limitId), ct);
}
