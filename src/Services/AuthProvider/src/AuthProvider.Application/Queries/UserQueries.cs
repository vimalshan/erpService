using AuthProvider.Application.DTOs;
using MediatR;

namespace AuthProvider.Application.Queries;

/// <summary>CQRS Query – returns a single user by ID (read side).</summary>
public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto?>;

/// <summary>CQRS Query – returns a user by email.</summary>
public record GetUserByEmailQuery(string Email) : IRequest<UserDto?>;

/// <summary>CQRS Query – paged list of all users.</summary>
public record GetAllUsersQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<UserDto>>;

/// <summary>CQRS Query – returns all roles.</summary>
public record GetAllRolesQuery() : IRequest<IEnumerable<RoleDto>>;

public record RoleDto(Guid Id, string Name, string Description);
