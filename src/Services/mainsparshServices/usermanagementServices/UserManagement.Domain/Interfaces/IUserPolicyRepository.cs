using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces;

public interface IUserPolicyRepository
{
    Task<UserPolicy?> GetByIdAsync(long policyId, CancellationToken cancellationToken = default);
    Task<UserPolicy?> GetByUserSysIdAsync(long userSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserPolicy>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<UserPolicy>> GetByPolicyTypeAsync(string policyType, CancellationToken cancellationToken = default);
    Task<UserPolicy> AddAsync(UserPolicy policy, CancellationToken cancellationToken = default);
    Task<UserPolicy> UpdateAsync(UserPolicy policy, CancellationToken cancellationToken = default);
    Task DeleteAsync(long policyId, CancellationToken cancellationToken = default);
    Task<bool> ExistsForUserAsync(long userSysId, CancellationToken cancellationToken = default);
}
