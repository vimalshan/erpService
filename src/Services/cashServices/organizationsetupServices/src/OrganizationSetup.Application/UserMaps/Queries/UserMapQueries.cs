using MediatR;
using OrganizationSetup.Application.DTOs;

namespace OrganizationSetup.Application.UserMaps.Queries;

public sealed record GetUserMapsByOrgQuery(long OrgId) : IRequest<IEnumerable<UserMapDto>>;
public sealed record GetUserMapsByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<UserMapDto>>;
