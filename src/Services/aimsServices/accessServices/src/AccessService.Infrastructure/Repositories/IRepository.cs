namespace AccessService.Infrastructure.Repositories;

using AccessService.Domain.Entities;

/// <summary>
/// Generic repository interface
/// </summary>
public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetAllAsync();
}

/// <summary>
/// Dedicated repository interfaces for domain aggregates
/// </summary>

public interface IUserMapRepository : IRepository<UserMap>
{
    Task<UserMap?> GetByEmployeeSystemIdAsync(long employeeSystemId);
    Task<IEnumerable<UserMap>> GetActiveUserMapsAsync();
}

public interface IUserRoleRepository : IRepository<UserRole>
{
    Task<UserRole?> GetByRoleIdAsync(int roleId);
    Task<IEnumerable<UserRole>> GetRolesByEmployeeIdAsync(long employeeSystemId);
    Task<IEnumerable<UserRole>> GetRolesByTypeAsync(char roleType);
    Task<IEnumerable<UserRole>> GetActiveRolesAsync();
}

public interface IMenuRepository : IRepository<Menu>
{
    Task<Menu?> GetByMenuIdAsync(int menuId);
    Task<IEnumerable<Menu>> GetRootMenusAsync();
    Task<IEnumerable<Menu>> GetMenusByParentIdAsync(int parentMenuId);
}

public interface ISPARSHMenuRepository : IRepository<SPARSHMenu>
{
    Task<SPARSHMenu?> GetByMenuIdAsync(long menuId);
    Task<SPARSHMenu?> GetByPageNameAsync(string pageName);
}

public interface ISPARSHMenuAccessRepository : IRepository<SPARSHMenuAccess>
{
    Task<SPARSHMenuAccess?> GetByAccessIdAsync(long accessId);
    Task<IEnumerable<SPARSHMenuAccess>> GetAccessByUnitAsync(long unitId);
    Task<IEnumerable<SPARSHMenuAccess>> GetAccessByCalendarAsync(long calendarId);
}
