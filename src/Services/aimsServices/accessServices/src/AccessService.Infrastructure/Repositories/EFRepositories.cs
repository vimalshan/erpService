namespace AccessService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using AccessService.Domain.Entities;
using AccessService.Infrastructure.Persistence;

/// <summary>
/// Generic Entity Framework repository implementation
/// </summary>
public abstract class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AccessServiceDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected EFRepository(AccessServiceDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(new[] { id });
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
}

/// <summary>
/// UserMap Entity Framework repository
/// </summary>
public class EFUserMapRepository : EFRepository<UserMap>, IUserMapRepository
{
    public EFUserMapRepository(AccessServiceDbContext context) : base(context)
    {
    }

    public async Task<UserMap?> GetByEmployeeSystemIdAsync(long employeeSystemId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.EmployeeSystemId == employeeSystemId);
    }

    public async Task<IEnumerable<UserMap>> GetActiveUserMapsAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(x => (x.EffectiveDate == null || x.EffectiveDate <= now) &&
                        (x.ClosureDate == null || x.ClosureDate > now))
            .ToListAsync();
    }
}

/// <summary>
/// UserRole Entity Framework repository
/// </summary>
public class EFUserRoleRepository : EFRepository<UserRole>, IUserRoleRepository
{
    public EFUserRoleRepository(AccessServiceDbContext context) : base(context)
    {
    }

    public async Task<UserRole?> GetByRoleIdAsync(int roleId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.RoleId == roleId);
    }

    public async Task<IEnumerable<UserRole>> GetRolesByEmployeeIdAsync(long employeeSystemId)
    {
        return await _dbSet
            .Where(x => x.EmployeeSystemId == employeeSystemId)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserRole>> GetRolesByTypeAsync(char roleType)
    {
        return await _dbSet
            .Where(x => x.RoleType == roleType)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserRole>> GetActiveRolesAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(x => (x.EffectiveDate == null || x.EffectiveDate <= now) &&
                        (x.ClosureDate == null || x.ClosureDate > now))
            .ToListAsync();
    }
}

/// <summary>
/// Menu Entity Framework repository
/// </summary>
public class EFMenuRepository : EFRepository<Menu>, IMenuRepository
{
    public EFMenuRepository(AccessServiceDbContext context) : base(context)
    {
    }

    public async Task<Menu?> GetByMenuIdAsync(int menuId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.MenuId == menuId);
    }

    public async Task<IEnumerable<Menu>> GetRootMenusAsync()
    {
        return await _dbSet
            .Where(x => x.ParentMenuId == null || x.ParentMenuId == 0)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<IEnumerable<Menu>> GetMenusByParentIdAsync(int parentMenuId)
    {
        return await _dbSet
            .Where(x => x.ParentMenuId == parentMenuId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync();
    }
}

/// <summary>
/// SPARSHMenu Entity Framework repository
/// </summary>
public class EFSPARSHMenuRepository : EFRepository<SPARSHMenu>, ISPARSHMenuRepository
{
    public EFSPARSHMenuRepository(AccessServiceDbContext context) : base(context)
    {
    }

    public async Task<SPARSHMenu?> GetByMenuIdAsync(long menuId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.MenuId == menuId);
    }

    public async Task<SPARSHMenu?> GetByPageNameAsync(string pageName)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.PageName == pageName);
    }
}

/// <summary>
/// SPARSHMenuAccess Entity Framework repository
/// </summary>
public class EFSPARSHMenuAccessRepository : EFRepository<SPARSHMenuAccess>, ISPARSHMenuAccessRepository
{
    public EFSPARSHMenuAccessRepository(AccessServiceDbContext context) : base(context)
    {
    }

    public async Task<SPARSHMenuAccess?> GetByAccessIdAsync(long accessId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.AccessId == accessId);
    }

    public async Task<IEnumerable<SPARSHMenuAccess>> GetAccessByUnitAsync(long unitId)
    {
        return await _dbSet
            .Where(x => x.UnitId == unitId)
            .ToListAsync();
    }

    public async Task<IEnumerable<SPARSHMenuAccess>> GetAccessByCalendarAsync(long calendarId)
    {
        return await _dbSet
            .Where(x => x.CalendarId == calendarId)
            .ToListAsync();
    }
}
