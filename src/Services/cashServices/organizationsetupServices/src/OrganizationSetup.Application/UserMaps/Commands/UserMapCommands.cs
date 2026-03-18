using MediatR;
using OrganizationSetup.Application.DTOs;

namespace OrganizationSetup.Application.UserMaps.Commands;

public sealed record CreateUserMapCommand(long MapId, long RoleId, long EmpSysId, long OrgId, long? Business = null) : IRequest<UserMapDto>;

public sealed record DeleteUserMapCommand(long MapId) : IRequest<bool>;
