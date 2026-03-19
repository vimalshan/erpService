using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Queries;

/// <summary>
/// Get User by ID query
/// </summary>
public class GetUserByIdQuery : IRequest<UserDto?>
{
    public long UserId { get; init; }
}

/// <summary>
/// Get User by Email query
/// </summary>
public class GetUserByEmailQuery : IRequest<UserDto?>
{
    public required string Email { get; init; }
}

/// <summary>
/// Get All Users query
/// </summary>
public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
{
}

/// <summary>
/// Get Active Users query
/// </summary>
public class GetActiveUsersQuery : IRequest<IEnumerable<UserDto>>
{
}

/// <summary>
/// Get Users by Role query
/// </summary>
public class GetUsersByRoleQuery : IRequest<IEnumerable<UserDto>>
{
    public long RoleId { get; init; }
}

/// <summary>
/// Get Users by Organization query
/// </summary>
public class GetUsersByOrganizationQuery : IRequest<IEnumerable<UserDto>>
{
    public required string BusinessUnitId { get; init; }
}

/// <summary>
/// Get Users by Location query
/// </summary>
public class GetUsersByLocationQuery : IRequest<IEnumerable<UserDto>>
{
    public int LocationId { get; init; }
}
