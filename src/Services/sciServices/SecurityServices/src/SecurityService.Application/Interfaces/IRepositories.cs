using SecurityService.Domain.Entities;

namespace SecurityService.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<User?> GetByCodeAsync(string userCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<User> Items, int TotalCount)> SearchAsync(string? searchTerm, bool activeOnly, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<long> AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long userId, CancellationToken cancellationToken = default);
}

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(long roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<long> AddAsync(Role role, CancellationToken cancellationToken = default);
    Task UpdateAsync(Role role, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> GetUserRolesAsync(long userId, CancellationToken cancellationToken = default);
    Task AssignRoleAsync(long userId, long roleId, DateTime startDate, DateTime? endDate, string assignedBy, CancellationToken cancellationToken = default);
    Task RevokeRoleAsync(long userId, long roleId, CancellationToken cancellationToken = default);
}

public interface IMenuRepository
{
    Task<IEnumerable<MenuMaster>> GetAllMenusAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuMaster>> GetMenusByRoleAsync(long roleId, CancellationToken cancellationToken = default);
    Task AssignMenuAsync(long roleId, long menuId, string assignedBy, long assignedByNum, CancellationToken cancellationToken = default);
    Task UnassignMenuAsync(long roleId, long menuId, CancellationToken cancellationToken = default);
    Task<bool> MenuExistsAsync(long menuId, CancellationToken cancellationToken = default);
    Task<bool> MenuAssignedToRoleAsync(long roleId, long menuId, CancellationToken cancellationToken = default);
}

public interface IUserMasterMapRepository
{
    Task<IEnumerable<UserMasterMap>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<UserMasterMap>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<UserMasterMap?> GetByIdAsync(long mapId, CancellationToken cancellationToken = default);
    Task<long> AddAsync(UserMasterMap map, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserMasterMap map, CancellationToken cancellationToken = default);
    Task DeleteAsync(long mapId, CancellationToken cancellationToken = default);
}
