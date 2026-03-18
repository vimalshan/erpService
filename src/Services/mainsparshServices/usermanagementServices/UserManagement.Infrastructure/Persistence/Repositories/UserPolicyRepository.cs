using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Infrastructure.Persistence.Repositories;

public class UserPolicyRepository(ApplicationDbContext context) : IUserPolicyRepository
{
    public async Task<UserPolicy?> GetByIdAsync(long policyId, CancellationToken cancellationToken = default)
        => await context.UserPolicies
            .Include(p => p.ProfileHistories)
            .FirstOrDefaultAsync(p => p.PolicyId == policyId, cancellationToken);

    public async Task<UserPolicy?> GetByUserSysIdAsync(long userSysId, CancellationToken cancellationToken = default)
        => await context.UserPolicies
            .Include(p => p.ProfileHistories)
            .FirstOrDefaultAsync(p => p.UserSysId == userSysId, cancellationToken);

    public async Task<IEnumerable<UserPolicy>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.UserPolicies.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<UserPolicy>> GetByPolicyTypeAsync(string policyType, CancellationToken cancellationToken = default)
        => await context.UserPolicies
            .AsNoTracking()
            .Where(p => p.PolicyType == policyType)
            .ToListAsync(cancellationToken);

    public async Task<UserPolicy> AddAsync(UserPolicy policy, CancellationToken cancellationToken = default)
    {
        await context.UserPolicies.AddAsync(policy, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return policy;
    }

    public async Task<UserPolicy> UpdateAsync(UserPolicy policy, CancellationToken cancellationToken = default)
    {
        context.UserPolicies.Update(policy);
        await context.SaveChangesAsync(cancellationToken);
        return policy;
    }

    public async Task DeleteAsync(long policyId, CancellationToken cancellationToken = default)
    {
        var policy = await context.UserPolicies.FindAsync([policyId], cancellationToken);
        if (policy is not null)
        {
            context.UserPolicies.Remove(policy);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsForUserAsync(long userSysId, CancellationToken cancellationToken = default)
        => await context.UserPolicies.AnyAsync(p => p.UserSysId == userSysId, cancellationToken);
}
