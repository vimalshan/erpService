using AuthProvider.Application.DTOs;
using AuthProvider.Application.Queries;
using HotChocolate.Authorization;
using MediatR;

namespace AuthProvider.API.GraphQL.Queries;

/// <summary>
/// GraphQL Query type – read-side operations exposed over /graphql.
/// Pattern: HotChocolate pure-code first approach.
/// </summary>
public sealed class UserQueryType
{
    /// <summary>Get a user by their ID.</summary>
    [Authorize]
    public async Task<UserDto?> GetUserById(
        [Service] IMediator mediator,
        Guid userId,
        CancellationToken ct)
        => await mediator.Send(new GetUserByIdQuery(userId), ct);

    /// <summary>Get a user by email.</summary>
    [Authorize]
    public async Task<UserDto?> GetUserByEmail(
        [Service] IMediator mediator,
        string email,
        CancellationToken ct)
        => await mediator.Send(new GetUserByEmailQuery(email), ct);

    /// <summary>Get a paged list of all users.</summary>
    [Authorize(Roles = new[] { "ADMIN" })]
    public async Task<PagedResult<UserDto>> GetUsers(
        [Service] IMediator mediator,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
        => await mediator.Send(new GetAllUsersQuery(page, pageSize), ct);

    /// <summary>Get all roles.</summary>
    [Authorize]
    public async Task<IEnumerable<RoleDto>> GetRoles(
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetAllRolesQuery(), ct);
}
