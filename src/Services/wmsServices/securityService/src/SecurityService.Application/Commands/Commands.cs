using MediatR;
using SecurityService.Application.DTOs;

namespace SecurityService.Application.Commands;

// User Commands
public record CreateUserCommand(UserCreateDto Dto) : IRequest<UserDto>;
public record UpdateUserCommand(UserUpdateDto Dto) : IRequest<UserDto>;
public record DeleteUserCommand(int UserId) : IRequest<Unit>;
public record AssignRoleToUserCommand(int UserId, int RoleId) : IRequest<Unit>;
public record RemoveRoleFromUserCommand(int UserId, int RoleId) : IRequest<Unit>;
public record DeactivateUserCommand(int UserId) : IRequest<Unit>;

// Role Commands
public record CreateRoleCommand(RoleCreateDto Dto) : IRequest<RoleDto>;
public record UpdateRoleCommand(RoleUpdateDto Dto) : IRequest<RoleDto>;
public record DeleteRoleCommand(int RoleId) : IRequest<Unit>;
public record AssignPermissionToRoleCommand(int RoleId, int PermissionId) : IRequest<Unit>;
public record RemovePermissionFromRoleCommand(int RoleId, int PermissionId) : IRequest<Unit>;

// Permission Commands
public record CreatePermissionCommand(PermissionCreateDto Dto) : IRequest<PermissionDto>;
public record UpdatePermissionCommand(PermissionUpdateDto Dto) : IRequest<PermissionDto>;
public record DeletePermissionCommand(int PermissionId) : IRequest<Unit>;

// Auth Commands
public record LoginCommand(LoginDto Dto) : IRequest<LoginResponseDto>;
