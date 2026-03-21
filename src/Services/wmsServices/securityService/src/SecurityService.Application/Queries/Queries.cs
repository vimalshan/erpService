using MediatR;
using SecurityService.Application.DTOs;

namespace SecurityService.Application.Queries;

public record GetUserByIdQuery(int UserId) : IRequest<UserDto?>;
public record GetUserByUsernameQuery(string Username) : IRequest<UserDto?>;
public record GetAllUsersQuery : IRequest<IReadOnlyList<UserDto>>;
public record GetRoleByIdQuery(int RoleId) : IRequest<RoleDto?>;
public record GetAllRolesQuery : IRequest<IReadOnlyList<RoleDto>>;
public record GetPermissionByIdQuery(int PermissionId) : IRequest<PermissionDto?>;
public record GetAllPermissionsQuery : IRequest<IReadOnlyList<PermissionDto>>;
public record GetPermissionsByModuleQuery(string Module) : IRequest<IReadOnlyList<PermissionDto>>;
