using MenuAndSecurityService.Domain.Entities;

namespace MenuAndSecurityService.Domain.Interfaces;

public interface IMenuRepository
{
    Task<MenuMaster?> GetByIdAsync(long menuId, CancellationToken ct = default);
    Task<IEnumerable<MenuMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<MenuMaster>> GetByParentIdAsync(long parentId, CancellationToken ct = default);
    Task<MenuMaster> AddAsync(MenuMaster menu, CancellationToken ct = default);
    Task UpdateAsync(MenuMaster menu, CancellationToken ct = default);
    Task DeleteAsync(long menuId, CancellationToken ct = default);
}
