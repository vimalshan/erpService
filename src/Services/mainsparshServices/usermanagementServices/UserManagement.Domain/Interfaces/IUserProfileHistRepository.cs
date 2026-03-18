using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces;

public interface IUserProfileHistRepository
{
    Task<UserProfileHistory?> GetByIdAsync(long histId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserProfileHistory>> GetByUserSysIdAsync(long userSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserProfileHistory>> GetByPolicyIdAsync(long policyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserProfileHistory>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<UserProfileHistory> AddAsync(UserProfileHistory history, CancellationToken cancellationToken = default);
}
