using SecurityService.Domain.Entities;

namespace SecurityService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default);
    Task<User> AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
    Task DeleteAsync(int userId, CancellationToken ct = default);
}

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int roleId, CancellationToken ct = default);
    Task<Role?> GetByNameAsync(string roleName, CancellationToken ct = default);
    Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken ct = default);
    Task<Role> AddAsync(Role role, CancellationToken ct = default);
    Task UpdateAsync(Role role, CancellationToken ct = default);
    Task DeleteAsync(int roleId, CancellationToken ct = default);
}

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(int permissionId, CancellationToken ct = default);
    Task<IReadOnlyList<Permission>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Permission>> GetByModuleAsync(string module, CancellationToken ct = default);
    Task<Permission> AddAsync(Permission permission, CancellationToken ct = default);
    Task UpdateAsync(Permission permission, CancellationToken ct = default);
    Task DeleteAsync(int permissionId, CancellationToken ct = default);
}

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IPermissionRepository Permissions { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
