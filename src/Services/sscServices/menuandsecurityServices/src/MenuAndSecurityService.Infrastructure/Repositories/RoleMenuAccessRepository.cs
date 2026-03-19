using MenuAndSecurityService.Domain.Entities;
using MenuAndSecurityService.Domain.Interfaces;
using MenuAndSecurityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MenuAndSecurityService.Infrastructure.Repositories;

public class RoleMenuAccessRepository : IRoleMenuAccessRepository
{
    private readonly MenuSecurityDbContext _context;

    public RoleMenuAccessRepository(MenuSecurityDbContext context)
    {
        _context = context;
    }

    public async Task<RoleMenuAccess?> GetByIdAsync(long accessId, CancellationToken ct = default)
    {
        return await _context.RoleMenuAccesses
            .Include(r => r.Menu)
            .FirstOrDefaultAsync(r => r.MenuAccessId == accessId, ct);
    }

    public async Task<IEnumerable<RoleMenuAccess>> GetByRoleIdAsync(long roleId, CancellationToken ct = default)
    {
        return await _context.RoleMenuAccesses
            .Include(r => r.Menu)
            .Where(r => r.MenuRoleId == roleId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<RoleMenuAccess>> GetByMenuIdAsync(long menuId, CancellationToken ct = default)
    {
        return await _context.RoleMenuAccesses
            .Include(r => r.Menu)
            .Where(r => r.MenuId == menuId)
            .ToListAsync(ct);
    }

    public async Task<RoleMenuAccess> AddAsync(RoleMenuAccess access, CancellationToken ct = default)
    {
        _context.RoleMenuAccesses.Add(access);
        await _context.SaveChangesAsync(ct);
        return access;
    }

    public async Task DeleteAsync(long accessId, CancellationToken ct = default)
    {
        var access = await _context.RoleMenuAccesses.FindAsync([accessId], ct);
        if (access is not null)
        {
            _context.RoleMenuAccesses.Remove(access);
            await _context.SaveChangesAsync(ct);
        }
    }
}
