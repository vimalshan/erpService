using MediatR;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;
using SecurityService.Domain.Interfaces;

namespace SecurityService.Application.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUnitOfWork _uow;
    public GetUserByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
        if (user is null) return null;

        var roles = user.UserRoles?.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>();
        return new UserDto(user.UserId, user.Username, user.Email, user.FullName, user.IsActive, user.CreatedDate, user.LastLogin, roles);
    }
}

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, UserDto?>
{
    private readonly IUnitOfWork _uow;
    public GetUserByUsernameQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<UserDto?> Handle(GetUserByUsernameQuery request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByUsernameAsync(request.Username, ct);
        if (user is null) return null;

        var roles = user.UserRoles?.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>();
        return new UserDto(user.UserId, user.Username, user.Email, user.FullName, user.IsActive, user.CreatedDate, user.LastLogin, roles);
    }
}

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly IUnitOfWork _uow;
    public GetAllUsersQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var users = await _uow.Users.GetAllAsync(ct);
        return users.Select(u => new UserDto(u.UserId, u.Username, u.Email, u.FullName, u.IsActive, u.CreatedDate, u.LastLogin,
            u.UserRoles?.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>())).ToList();
    }
}

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IUnitOfWork _uow;
    public GetRoleByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken ct)
    {
        var role = await _uow.Roles.GetByIdAsync(request.RoleId, ct);
        if (role is null) return null;

        var perms = role.RolePermissions?.Select(rp => rp.Permission.PermissionName).ToList() ?? new List<string>();
        return new RoleDto(role.RoleId, role.RoleName, role.Description, perms);
    }
}

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IReadOnlyList<RoleDto>>
{
    private readonly IUnitOfWork _uow;
    public GetAllRolesQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken ct)
    {
        var roles = await _uow.Roles.GetAllAsync(ct);
        return roles.Select(r => new RoleDto(r.RoleId, r.RoleName, r.Description,
            r.RolePermissions?.Select(rp => rp.Permission.PermissionName).ToList() ?? new List<string>())).ToList();
    }
}

public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto?>
{
    private readonly IUnitOfWork _uow;
    public GetPermissionByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PermissionDto?> Handle(GetPermissionByIdQuery request, CancellationToken ct)
    {
        var perm = await _uow.Permissions.GetByIdAsync(request.PermissionId, ct);
        if (perm is null) return null;
        return new PermissionDto(perm.PermissionId, perm.PermissionName, perm.Module, perm.Description);
    }
}

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IReadOnlyList<PermissionDto>>
{
    private readonly IUnitOfWork _uow;
    public GetAllPermissionsQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken ct)
    {
        var perms = await _uow.Permissions.GetAllAsync(ct);
        return perms.Select(p => new PermissionDto(p.PermissionId, p.PermissionName, p.Module, p.Description)).ToList();
    }
}

public class GetPermissionsByModuleQueryHandler : IRequestHandler<GetPermissionsByModuleQuery, IReadOnlyList<PermissionDto>>
{
    private readonly IUnitOfWork _uow;
    public GetPermissionsByModuleQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<PermissionDto>> Handle(GetPermissionsByModuleQuery request, CancellationToken ct)
    {
        var perms = await _uow.Permissions.GetByModuleAsync(request.Module, ct);
        return perms.Select(p => new PermissionDto(p.PermissionId, p.PermissionName, p.Module, p.Description)).ToList();
    }
}
