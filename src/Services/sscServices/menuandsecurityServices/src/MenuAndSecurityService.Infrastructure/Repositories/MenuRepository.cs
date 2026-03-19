using MenuAndSecurityService.Domain.Entities;
using MenuAndSecurityService.Domain.Interfaces;
using MenuAndSecurityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MenuAndSecurityService.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly MenuSecurityDbContext _context;

    public MenuRepository(MenuSecurityDbContext context)
    {
        _context = context;
    }

    public async Task<MenuMaster?> GetByIdAsync(long menuId, CancellationToken ct = default)
    {
        return await _context.MenuMasters
            .Include(m => m.Children)
            .Include(m => m.RoleMenuAccesses)
            .FirstOrDefaultAsync(m => m.MenuId == menuId, ct);
    }

    public async Task<IEnumerable<MenuMaster>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.MenuMasters
            .Include(m => m.Children)
            .OrderBy(m => m.MenuDisplayOrder)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<MenuMaster>> GetByParentIdAsync(long parentId, CancellationToken ct = default)
    {
        return await _context.MenuMasters
            .Where(m => m.MenuParentId == parentId)
            .OrderBy(m => m.MenuDisplayOrder)
            .ToListAsync(ct);
    }

    public async Task<MenuMaster> AddAsync(MenuMaster menu, CancellationToken ct = default)
    {
        _context.MenuMasters.Add(menu);
        await _context.SaveChangesAsync(ct);
        return menu;
    }

    public async Task UpdateAsync(MenuMaster menu, CancellationToken ct = default)
    {
        _context.MenuMasters.Update(menu);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long menuId, CancellationToken ct = default)
    {
        var menu = await _context.MenuMasters.FindAsync([menuId], ct);
        if (menu is not null)
        {
            _context.MenuMasters.Remove(menu);
            await _context.SaveChangesAsync(ct);
        }
    }
}
