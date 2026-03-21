using SecurityService.Domain.Interfaces;
using SecurityService.Infrastructure.Persistence;

namespace SecurityService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SecurityDbContext _db;
    private IUserRepository? _users;
    private IRoleRepository? _roles;
    private IPermissionRepository? _permissions;

    public UnitOfWork(SecurityDbContext db) => _db = db;

    public IUserRepository Users => _users ??= new UserRepository(_db);
    public IRoleRepository Roles => _roles ??= new RoleRepository(_db);
    public IPermissionRepository Permissions => _permissions ??= new PermissionRepository(_db);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);

    public void Dispose() => _db.Dispose();
}
