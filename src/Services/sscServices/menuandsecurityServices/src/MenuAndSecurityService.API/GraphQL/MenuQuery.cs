using MenuAndSecurityService.Domain.Entities;
using MenuAndSecurityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MenuAndSecurityService.API.GraphQL;

public class MenuQuery
{
    [UseFiltering]
    [UseSorting]
    public IQueryable<MenuMaster> GetMenus([Service] MenuSecurityDbContext context)
    {
        return context.MenuMasters.Include(m => m.Children).Include(m => m.RoleMenuAccesses);
    }

    public async Task<MenuMaster?> GetMenuById([Service] MenuSecurityDbContext context, long menuId)
    {
        return await context.MenuMasters
            .Include(m => m.Children)
            .Include(m => m.RoleMenuAccesses)
            .FirstOrDefaultAsync(m => m.MenuId == menuId);
    }

    [UseFiltering]
    [UseSorting]
    public IQueryable<RoleMenuAccess> GetRoleMenuAccesses([Service] MenuSecurityDbContext context)
    {
        return context.RoleMenuAccesses.Include(r => r.Menu);
    }

    public async Task<IEnumerable<RoleMenuAccess>> GetMenusByRoleId([Service] MenuSecurityDbContext context, long roleId)
    {
        return await context.RoleMenuAccesses
            .Include(r => r.Menu)
            .Where(r => r.MenuRoleId == roleId)
            .ToListAsync();
    }
}
