using MediatR;
using OrganizationSetup.Application.DTOs;

namespace OrganizationSetup.Application.Roles.Queries;

public sealed record GetRolesQuery : IRequest<IEnumerable<RoleDto>>;

public sealed record GetRoleByIdQuery(long RoleId) : IRequest<RoleDto?>;
