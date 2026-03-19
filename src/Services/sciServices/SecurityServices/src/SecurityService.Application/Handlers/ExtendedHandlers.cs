using MediatR;
using SecurityService.Application.Commands.Users;
using SecurityService.Application.DTOs;
using SecurityService.Application.Interfaces;
using SecurityService.Application.Queries;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Exceptions;

namespace SecurityService.Application.Handlers;

// ── UserMasterMap Handlers ────────────────────────────────────────────────

public sealed class CreateUserMapHandler : IRequestHandler<CreateUserMapCommand, UserMasterMapDto>
{
    private readonly IUserMasterMapRepository _maps;
    private readonly IUserRepository _users;

    public CreateUserMapHandler(IUserMasterMapRepository maps, IUserRepository users)
    {
        _maps = maps;
        _users = users;
    }

    public async Task<UserMasterMapDto> Handle(CreateUserMapCommand request, CancellationToken cancellationToken)
    {
        if (!await _users.ExistsAsync(request.UserId, cancellationToken))
            throw new UserNotFoundException(request.UserId);

        var map = new UserMasterMap
        {
            UserId = request.UserId,
            DepartmentCode = request.DepartmentCode,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        var newId = await _maps.AddAsync(map, cancellationToken);
        map.MapId = newId;

        return new(map.MapId, map.UserId, map.DepartmentCode, map.StartDate, map.EndDate);
    }
}

public sealed class UpdateUserMapHandler : IRequestHandler<UpdateUserMapCommand, UserMasterMapDto>
{
    private readonly IUserMasterMapRepository _maps;

    public UpdateUserMapHandler(IUserMasterMapRepository maps) => _maps = maps;

    public async Task<UserMasterMapDto> Handle(UpdateUserMapCommand request, CancellationToken cancellationToken)
    {
        var map = await _maps.GetByIdAsync(request.MapId, cancellationToken)
            ?? throw new DomainException($"User map {request.MapId} not found.");

        map.DepartmentCode = request.DepartmentCode;
        map.StartDate = request.StartDate;
        map.EndDate = request.EndDate;

        await _maps.UpdateAsync(map, cancellationToken);

        return new(map.MapId, map.UserId, map.DepartmentCode, map.StartDate, map.EndDate);
    }
}

public sealed class DeleteUserMapHandler : IRequestHandler<DeleteUserMapCommand, bool>
{
    private readonly IUserMasterMapRepository _maps;

    public DeleteUserMapHandler(IUserMasterMapRepository maps) => _maps = maps;

    public async Task<bool> Handle(DeleteUserMapCommand request, CancellationToken cancellationToken)
    {
        await _maps.DeleteAsync(request.MapId, cancellationToken);
        return true;
    }
}

// ── Menu-Role Assignment Handlers ─────────────────────────────────────────

public sealed class AssignMenuToRoleHandler : IRequestHandler<AssignMenuToRoleCommand, bool>
{
    private readonly IMenuRepository _menus;
    private readonly IRoleRepository _roles;

    public AssignMenuToRoleHandler(IMenuRepository menus, IRoleRepository roles)
    {
        _menus = menus;
        _roles = roles;
    }

    public async Task<bool> Handle(AssignMenuToRoleCommand request, CancellationToken cancellationToken)
    {
        if (!await _roles.ExistsAsync(request.RoleId, cancellationToken))
            throw new RoleNotFoundException(request.RoleId);

        if (!await _menus.MenuExistsAsync(request.MenuId, cancellationToken))
            throw new DomainException($"Menu {request.MenuId} not found.");

        if (await _menus.MenuAssignedToRoleAsync(request.RoleId, request.MenuId, cancellationToken))
            throw new DomainException($"Menu {request.MenuId} is already assigned to role {request.RoleId}.");

        await _menus.AssignMenuAsync(request.RoleId, request.MenuId,
            request.AssignedBy, request.AssignedByNum, cancellationToken);

        return true;
    }
}

public sealed class UnassignMenuFromRoleHandler : IRequestHandler<UnassignMenuFromRoleCommand, bool>
{
    private readonly IMenuRepository _menus;

    public UnassignMenuFromRoleHandler(IMenuRepository menus) => _menus = menus;

    public async Task<bool> Handle(UnassignMenuFromRoleCommand request, CancellationToken cancellationToken)
    {
        await _menus.UnassignMenuAsync(request.RoleId, request.MenuId, cancellationToken);
        return true;
    }
}

// ── UserMasterMap Query Handlers ──────────────────────────────────────────

public sealed class GetUserMapsHandler : IRequestHandler<GetUserMapsQuery, IEnumerable<UserMasterMapDto>>
{
    private readonly IUserMasterMapRepository _maps;

    public GetUserMapsHandler(IUserMasterMapRepository maps) => _maps = maps;

    public async Task<IEnumerable<UserMasterMapDto>> Handle(GetUserMapsQuery request, CancellationToken cancellationToken)
    {
        var maps = await _maps.GetAllAsync(cancellationToken);
        return maps.Select(m => new UserMasterMapDto(m.MapId, m.UserId, m.DepartmentCode, m.StartDate, m.EndDate));
    }
}

public sealed class GetUserMapsByUserHandler : IRequestHandler<GetUserMapsByUserQuery, IEnumerable<UserMasterMapDto>>
{
    private readonly IUserMasterMapRepository _maps;

    public GetUserMapsByUserHandler(IUserMasterMapRepository maps) => _maps = maps;

    public async Task<IEnumerable<UserMasterMapDto>> Handle(GetUserMapsByUserQuery request, CancellationToken cancellationToken)
    {
        var maps = await _maps.GetByUserIdAsync(request.UserId, cancellationToken);
        return maps.Select(m => new UserMasterMapDto(m.MapId, m.UserId, m.DepartmentCode, m.StartDate, m.EndDate));
    }
}

public sealed class GetUserMapByIdHandler : IRequestHandler<GetUserMapByIdQuery, UserMasterMapDto?>
{
    private readonly IUserMasterMapRepository _maps;

    public GetUserMapByIdHandler(IUserMasterMapRepository maps) => _maps = maps;

    public async Task<UserMasterMapDto?> Handle(GetUserMapByIdQuery request, CancellationToken cancellationToken)
    {
        var map = await _maps.GetByIdAsync(request.MapId, cancellationToken);
        return map is null ? null : new(map.MapId, map.UserId, map.DepartmentCode, map.StartDate, map.EndDate);
    }
}

// ── Search Users Handler ──────────────────────────────────────────────────

public sealed class SearchUsersHandler : IRequestHandler<SearchUsersQuery, PagedResult<UserListDto>>
{
    private readonly IUserRepository _users;

    public SearchUsersHandler(IUserRepository users) => _users = users;

    public async Task<PagedResult<UserListDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 200 ? 20 : request.PageSize;

        var (items, total) = await _users.SearchAsync(
            request.SearchTerm, request.ActiveOnly, page, pageSize, cancellationToken);

        var dtos = items.Select(u => new UserListDto(u.UserId, u.UserCode, u.UserName, u.Email?.Value, u.IsActive));
        return new PagedResult<UserListDto>(dtos, total, page, pageSize);
    }
}

// ── Access Tree Query Handler ─────────────────────────────────────────────

public sealed class GetUserAccessTreeHandler : IRequestHandler<GetUserAccessTreeQuery, UserAccessTreeDto>
{
    private readonly IUserRepository _users;
    private readonly IRoleRepository _roles;
    private readonly IMenuRepository _menus;

    public GetUserAccessTreeHandler(IUserRepository users, IRoleRepository roles, IMenuRepository menus)
    {
        _users = users;
        _roles = roles;
        _menus = menus;
    }

    public async Task<UserAccessTreeDto> Handle(GetUserAccessTreeQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        var userRoles = await _roles.GetUserRolesAsync(request.UserId, cancellationToken);

        var rolesWithMenus = new List<RoleWithMenusDto>();

        foreach (var ur in userRoles.Where(r => r.EndDate == null || r.EndDate >= DateTime.UtcNow))
        {
            var role = await _roles.GetByIdAsync(ur.RoleId, cancellationToken);
            var roleMenus = await _menus.GetMenusByRoleAsync(ur.RoleId, cancellationToken);

            rolesWithMenus.Add(new RoleWithMenusDto(
                ur.RoleId,
                role?.RoleName ?? string.Empty,
                roleMenus.Select(m => new MenuDto(m.MenuId ?? 0, m.MenuName, m.Url, m.ParentMenuId, m.DisplayOrder))));
        }

        return new UserAccessTreeDto(user.UserId, user.UserCode, user.UserName, rolesWithMenus);
    }
}

// ── Security Stats Handler ────────────────────────────────────────────────

public sealed class GetSecurityStatsHandler : IRequestHandler<GetSecurityStatsQuery, SecurityStatsDto>
{
    private readonly IUserRepository _users;
    private readonly IRoleRepository _roles;
    private readonly IMenuRepository _menus;

    public GetSecurityStatsHandler(IUserRepository users, IRoleRepository roles, IMenuRepository menus)
    {
        _users = users;
        _roles = roles;
        _menus = menus;
    }

    public async Task<SecurityStatsDto> Handle(GetSecurityStatsQuery request, CancellationToken cancellationToken)
    {
        var (_, totalUsers) = await _users.SearchAsync(null, false, 1, 1, cancellationToken);
        var (_, activeUsers) = await _users.SearchAsync(null, true, 1, 1, cancellationToken);

        var allRoles = await _roles.GetAllAsync(cancellationToken);
        var allMenus = await _menus.GetAllMenusAsync(cancellationToken);

        return new SecurityStatsDto(
            TotalUsers: totalUsers,
            ActiveUsers: activeUsers,
            TotalRoles: allRoles.Count(),
            TotalMenus: allMenus.Count(),
            GeneratedAt: DateTime.UtcNow);
    }
}
