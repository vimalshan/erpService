using MediatR;
using OrganizationSetup.Application.DTOs;

namespace OrganizationSetup.Application.Roles.Commands;

public sealed record CreateRoleCommand(
    long RoleId,
    string RoleName,
    long RoleLevel
) : IRequest<RoleDto>;
