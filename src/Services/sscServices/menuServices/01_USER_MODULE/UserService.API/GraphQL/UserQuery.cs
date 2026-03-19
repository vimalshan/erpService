using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Queries;

namespace UserService.API.GraphQL;

/// <summary>
/// GraphQL Query root — all read operations.
/// Access via POST /graphql with a JSON body: { "query": "{ users { userId userName } }" }
/// Interactive playground: /graphql/ui
/// </summary>
public class UserQuery
{
    /// <summary>
    /// Returns all users in the system.
    /// </summary>
    public async Task<IEnumerable<UserDto>> GetUsersAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetAllUsersQuery(), cancellationToken);

    /// <summary>
    /// Returns only active users.
    /// </summary>
    public async Task<IEnumerable<UserDto>> GetActiveUsersAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetActiveUsersQuery(), cancellationToken);

    /// <summary>
    /// Returns a single user by their numeric ID.
    /// </summary>
    public async Task<UserDto?> GetUserByIdAsync(
        long userId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetUserByIdQuery { UserId = userId }, cancellationToken);

    /// <summary>
    /// Returns a single user by email address.
    /// </summary>
    public async Task<UserDto?> GetUserByEmailAsync(
        string email,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetUserByEmailQuery { Email = email }, cancellationToken);

    /// <summary>
    /// Returns all users that have a specific role.
    /// </summary>
    public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(
        long roleId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetUsersByRoleQuery { RoleId = roleId }, cancellationToken);

    /// <summary>
    /// Returns all users belonging to a business unit (organisation).
    /// </summary>
    public async Task<IEnumerable<UserDto>> GetUsersByOrganizationAsync(
        string businessUnitId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new GetUsersByOrganizationQuery { BusinessUnitId = businessUnitId },
            cancellationToken);

    /// <summary>
    /// Returns all users assigned to a specific location.
    /// </summary>
    public async Task<IEnumerable<UserDto>> GetUsersByLocationAsync(
        int locationId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new GetUsersByLocationQuery { LocationId = locationId },
            cancellationToken);
}
