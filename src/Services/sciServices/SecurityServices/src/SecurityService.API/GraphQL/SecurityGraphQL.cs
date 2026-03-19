using MediatR;
using SecurityService.Application.Commands.Users;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;

namespace SecurityService.API.GraphQL;

/// <summary>HotChocolate GraphQL Query type.</summary>
public sealed class SecurityQuery
{
    public async Task<IEnumerable<UserListDto>> GetUsers(
        [Service] IMediator mediator,
        bool activeOnly = false,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllUsersQuery(activeOnly), cancellationToken);

    public async Task<UserDto?> GetUser(
        long id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetUserByIdQuery(id), cancellationToken);

    public async Task<IEnumerable<RoleDto>> GetRoles(
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllRolesQuery(), cancellationToken);

    public async Task<RoleDto?> GetRole(
        long id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetRoleByIdQuery(id), cancellationToken);

    public async Task<IEnumerable<UserRoleDto>> GetUserRoles(
        long userId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetUserRolesQuery(userId), cancellationToken);

    public async Task<IEnumerable<MenuDto>> GetMenus(
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllMenusQuery(), cancellationToken);

    public async Task<IEnumerable<MenuDto>> GetMenusByRole(
        long roleId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetMenusByRoleQuery(roleId), cancellationToken);
}

/// <summary>HotChocolate GraphQL Mutation type.</summary>
public sealed class SecurityMutation
{
    public async Task<UserDto> CreateUser(
        CreateUserCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);

    public async Task<RoleDto> CreateRole(
        CreateRoleCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);

    public async Task<bool> AssignRole(
        AssignRoleCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);

    public async Task<bool> RevokeRole(
        RevokeRoleCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);
}
