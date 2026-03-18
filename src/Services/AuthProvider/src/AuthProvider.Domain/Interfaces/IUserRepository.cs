using AuthProvider.Domain.Entities;

namespace AuthProvider.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<bool> ExistsAsync(string email, CancellationToken ct = default);
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken ct = default);
    Task<User?> GetWithRolesAsync(Guid userId, CancellationToken ct = default);
}
