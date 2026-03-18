using MediatR;
using OrganizationSetup.Application.DTOs;

namespace OrganizationSetup.Application.OrgParams.Commands;

public sealed record CreateOrgParamCommand(long ParamId, string ParamType, long ParamValue, long OrgId) : IRequest<OrgParamsDto>;
public sealed record UpdateOrgParamCommand(long ParamId, long NewValue) : IRequest<OrgParamsDto>;
