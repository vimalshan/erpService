using Microsoft.EntityFrameworkCore;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Interfaces;
using SecurityService.Infrastructure.Persistence;

namespace SecurityService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SecurityDbContext _db;
    public UserRepository(SecurityDbContext db) => _db = db;

    public async Task<User?> GetByIdAsync(int userId, CancellationToken ct = default) =>
        await _db.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.UserId == userId, ct);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        await _db.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == username, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .AsNoTracking().ToListAsync(ct);

    public async Task<User> AddAsync(User user, CancellationToken ct = default)
    {
        var entry = await _db.Users.AddAsync(user, ct);
        return entry.Entity;
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int userId, CancellationToken ct = default)
    {
        var user = await _db.Users.FindAsync(new object[] { userId }, ct);
        if (user is not null) _db.Users.Remove(user);
    }
}

public class RoleRepository : IRoleRepository
{
    private readonly SecurityDbContext _db;
    public RoleRepository(SecurityDbContext db) => _db = db;

    public async Task<Role?> GetByIdAsync(int roleId, CancellationToken ct = default) =>
        await _db.Roles
            .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.RoleId == roleId, ct);

    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken ct = default) =>
        await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName, ct);

    public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Roles
            .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .AsNoTracking().ToListAsync(ct);

    public async Task<Role> AddAsync(Role role, CancellationToken ct = default)
    {
        var entry = await _db.Roles.AddAsync(role, ct);
        return entry.Entity;
    }

    public Task UpdateAsync(Role role, CancellationToken ct = default)
    {
        _db.Roles.Update(role);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int roleId, CancellationToken ct = default)
    {
        var role = await _db.Roles.FindAsync(new object[] { roleId }, ct);
        if (role is not null) _db.Roles.Remove(role);
    }
}

public class PermissionRepository : IPermissionRepository
{
    private readonly SecurityDbContext _db;
    public PermissionRepository(SecurityDbContext db) => _db = db;

    public async Task<Permission?> GetByIdAsync(int permissionId, CancellationToken ct = default) =>
        await _db.Permissions.FindAsync(new object[] { permissionId }, ct);

    public async Task<IReadOnlyList<Permission>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Permissions.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<Permission>> GetByModuleAsync(string module, CancellationToken ct = default) =>
        await _db.Permissions.Where(p => p.Module == module).AsNoTracking().ToListAsync(ct);

    public async Task<Permission> AddAsync(Permission permission, CancellationToken ct = default)
    {
        var entry = await _db.Permissions.AddAsync(permission, ct);
        return entry.Entity;
    }

    public Task UpdateAsync(Permission permission, CancellationToken ct = default)
    {
        _db.Permissions.Update(permission);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int permissionId, CancellationToken ct = default)
    {
        var perm = await _db.Permissions.FindAsync(new object[] { permissionId }, ct);
        if (perm is not null) _db.Permissions.Remove(perm);
    }
}
