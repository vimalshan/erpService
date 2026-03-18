using AuthProvider.Application.DTOs;
using AuthProvider.Application.Queries;
using AuthProvider.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthProvider.Application.Handlers;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUnitOfWork _uow;

    public GetUserByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await _uow.Users.GetWithRolesAsync(request.UserId, ct);
        if (user is null) return null;

        return new UserDto(user.Id, user.Username, user.Email.Value,
            user.FirstName, user.LastName,
            user.IsActive, user.IsEmailVerified,
            user.CreatedAt, user.LastLoginAt,
            user.UserRoles.Select(ur => ur.Role?.Name ?? string.Empty));
    }
}

public sealed class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto?>
{
    private readonly IUnitOfWork _uow;

    public GetUserByEmailQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByEmailAsync(request.Email, ct);
        if (user is null) return null;

        return new UserDto(user.Id, user.Username, user.Email.Value,
            user.FirstName, user.LastName,
            user.IsActive, user.IsEmailVerified,
            user.CreatedAt, user.LastLoginAt,
            user.UserRoles.Select(ur => ur.Role?.Name ?? string.Empty));
    }
}

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<UserDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(IUnitOfWork uow, ILogger<GetAllUsersQueryHandler> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<PagedResult<UserDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var users = (await _uow.Users.GetAllAsync(ct)).ToList();
        var totalCount = users.Count;

        var paged = users
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new UserDto(u.Id, u.Username, u.Email.Value,
                u.FirstName, u.LastName,
                u.IsActive, u.IsEmailVerified,
                u.CreatedAt, u.LastLoginAt,
                u.UserRoles.Select(ur => ur.Role?.Name ?? string.Empty)))
            .ToList();

        _logger.LogDebug("Returning {Count}/{Total} users (page {Page})", paged.Count, totalCount, request.Page);
        return new PagedResult<UserDto>(paged, totalCount, request.Page, request.PageSize);
    }
}

public sealed class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllRolesQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken ct)
    {
        var roles = await _uow.Roles.GetAllAsync(ct);
        return roles.Select(r => new RoleDto(r.Id, r.Name, r.Description));
    }
}
