using MediatR;
using MenuAndSecurityService.Application.DTOs;

namespace MenuAndSecurityService.Application.Commands.GrantMenuAccess;

public sealed record GrantMenuAccessCommand(
    long MenuAccessId,
    long MenuId,
    long MenuRoleId,
    long ModifiedBy
) : IRequest<RoleMenuAccessDto>;
