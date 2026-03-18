using MediatR;
using OrganizationSetup.Application.DTOs;

namespace OrganizationSetup.Application.OrgParams.Queries;

public sealed record GetOrgParamsByOrgQuery(long OrgId) : IRequest<IEnumerable<OrgParamsDto>>;
public sealed record GetOrgParamByTypeQuery(long OrgId, string ParamType) : IRequest<OrgParamsDto?>;
