using MenuAndSecurityService.Domain.Entities;

namespace MenuAndSecurityService.Domain.Interfaces;

public interface IRoleMenuAccessRepository
{
    Task<RoleMenuAccess?> GetByIdAsync(long accessId, CancellationToken ct = default);
    Task<IEnumerable<RoleMenuAccess>> GetByRoleIdAsync(long roleId, CancellationToken ct = default);
    Task<IEnumerable<RoleMenuAccess>> GetByMenuIdAsync(long menuId, CancellationToken ct = default);
    Task<RoleMenuAccess> AddAsync(RoleMenuAccess access, CancellationToken ct = default);
    Task DeleteAsync(long accessId, CancellationToken ct = default);
}
