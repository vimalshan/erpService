using MediatR;
using MenuAndSecurityService.Application.DTOs;

namespace MenuAndSecurityService.Application.Queries.GetMenusByRole;

public sealed record GetMenusByRoleQuery(long RoleId) : IRequest<IEnumerable<RoleMenuAccessDto>>;
