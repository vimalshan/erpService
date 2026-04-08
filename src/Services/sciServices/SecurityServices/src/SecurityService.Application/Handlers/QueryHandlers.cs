using MediatR;
using SecurityService.Application.DTOs;
using SecurityService.Application.Interfaces;
using SecurityService.Application.Queries;
using SecurityService.Domain.Exceptions;

namespace SecurityService.Application.Handlers.Queries;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _users;
    public GetUserByIdHandler(IUserRepository users) => _users = users;

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null) return null;
        return new(user.UserId, user.UserCode, user.UserName, user.Email?.Value,
            user.Phone?.Value, user.StartDate, user.EndDate, user.UserType?.ToString(), user.IsActive);
    }
}

public sealed class GetUserByCodeHandler : IRequestHandler<GetUserByCodeQuery, UserDto?>
{
    private readonly IUserRepository _users;
    public GetUserByCodeHandler(IUserRepository users) => _users = users;

    public async Task<UserDto?> Handle(GetUserByCodeQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByCodeAsync(request.UserCode, cancellationToken);
        if (user is null) return null;
        return new(user.UserId, user.UserCode, user.UserName, user.Email?.Value,
            user.Phone?.Value, user.StartDate, user.EndDate, user.UserType?.ToString(), user.IsActive);
    }
}

public sealed class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserListDto>>
{
    private readonly IUserRepository _users;
    public GetAllUsersHandler(IUserRepository users) => _users = users;

    public async Task<IEnumerable<UserListDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _users.GetAllAsync(cancellationToken);
        var result = users.Select(u => new UserListDto(u.UserId, u.UserCode, u.UserName, u.Email?.Value, u.IsActive));
        return request.ActiveOnly ? result.Where(u => u.IsActive) : result;
    }
}

public sealed class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRoleRepository _roles;
    public GetAllRolesHandler(IRoleRepository roles) => _roles = roles;

    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roles.GetAllAsync(cancellationToken);
        return roles.Select(r => new RoleDto(r.RoleId, r.RoleName, r.UpdatedByCode, r.UpdatedAt));
    }
}

public sealed class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IRoleRepository _roles;
    public GetRoleByIdHandler(IRoleRepository roles) => _roles = roles;

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roles.GetByIdAsync(request.RoleId, cancellationToken);
        return role is null ? null : new(role.RoleId, role.RoleName, role.UpdatedByCode, role.UpdatedAt);
    }
}

public sealed class GetUserRolesHandler : IRequestHandler<GetUserRolesQuery, IEnumerable<UserRoleDto>>
{
    private readonly IRoleRepository _roles;
    public GetUserRolesHandler(IRoleRepository roles) => _roles = roles;

    public async Task<IEnumerable<UserRoleDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await _roles.GetUserRolesAsync(request.UserId, cancellationToken);
        return userRoles.Select(ur => new UserRoleDto(ur.UserId, ur.RoleId, ur.Role?.RoleName, ur.StartDate, ur.EndDate));
    }
}

public sealed class GetAllMenusHandler : IRequestHandler<GetAllMenusQuery, IEnumerable<MenuDto>>
{
    private readonly IMenuRepository _menus;
    public GetAllMenusHandler(IMenuRepository menus) => _menus = menus;

    public async Task<IEnumerable<MenuDto>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
    {
        var menus = await _menus.GetAllMenusAsync(cancellationToken);
        return menus.Select(m => new MenuDto(m.MenuId ?? 0, m.MenuName, m.Url, m.ParentMenuId, m.DisplayOrder));
    }
}

public sealed class GetMenusByRoleHandler : IRequestHandler<GetMenusByRoleQuery, IEnumerable<MenuDto>>
{
    private readonly IMenuRepository _menus;
    public GetMenusByRoleHandler(IMenuRepository menus) => _menus = menus;

    public async Task<IEnumerable<MenuDto>> Handle(GetMenusByRoleQuery request, CancellationToken cancellationToken)
    {
        var menus = await _menus.GetMenusByRoleAsync(request.RoleId, cancellationToken);
        return menus.Select(m => new MenuDto(m.MenuId ?? 0, m.MenuName, m.Url, m.ParentMenuId, m.DisplayOrder));
    }
}
