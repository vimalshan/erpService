using MediatR;
using SecurityService.Application.Commands;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;

namespace SecurityService.API.GraphQL;

public class SecurityQuery
{
    public async Task<IReadOnlyList<UserDto>> GetUsers([Service] IMediator mediator) =>
        await mediator.Send(new GetAllUsersQuery());

    public async Task<UserDto?> GetUserById([Service] IMediator mediator, int userId) =>
        await mediator.Send(new GetUserByIdQuery(userId));

    public async Task<IReadOnlyList<RoleDto>> GetRoles([Service] IMediator mediator) =>
        await mediator.Send(new GetAllRolesQuery());

    public async Task<RoleDto?> GetRoleById([Service] IMediator mediator, int roleId) =>
        await mediator.Send(new GetRoleByIdQuery(roleId));

    public async Task<IReadOnlyList<PermissionDto>> GetPermissions([Service] IMediator mediator) =>
        await mediator.Send(new GetAllPermissionsQuery());

    public async Task<PermissionDto?> GetPermissionById([Service] IMediator mediator, int permissionId) =>
        await mediator.Send(new GetPermissionByIdQuery(permissionId));

    public async Task<IReadOnlyList<PermissionDto>> GetPermissionsByModule([Service] IMediator mediator, string module) =>
        await mediator.Send(new GetPermissionsByModuleQuery(module));
}

public class SecurityMutation
{
    public async Task<UserDto> CreateUser([Service] IMediator mediator, UserCreateDto input) =>
        await mediator.Send(new CreateUserCommand(input));

    public async Task<LoginResponseDto> Login([Service] IMediator mediator, LoginDto input) =>
        await mediator.Send(new LoginCommand(input));

    public async Task<RoleDto> CreateRole([Service] IMediator mediator, RoleCreateDto input) =>
        await mediator.Send(new CreateRoleCommand(input));

    public async Task<PermissionDto> CreatePermission([Service] IMediator mediator, PermissionCreateDto input) =>
        await mediator.Send(new CreatePermissionCommand(input));
}
