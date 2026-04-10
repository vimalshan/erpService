using UserService.Domain.Entities;

namespace UserService.Domain.Repositories;

/// <summary>
/// Repository interface for User aggregate
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetByRoleAsync(long roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetByOrganizationAsync(string businessUnitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetByLocationAsync(int locationId, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(long userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Unit of Work pattern interface
/// </summary>
public interface IUnitOfWork
{
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
