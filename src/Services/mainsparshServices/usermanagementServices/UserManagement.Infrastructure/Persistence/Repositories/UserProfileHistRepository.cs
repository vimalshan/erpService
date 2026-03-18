using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Infrastructure.Persistence.Repositories;

public class UserProfileHistRepository(ApplicationDbContext context) : IUserProfileHistRepository
{
    public async Task<UserProfileHistory?> GetByIdAsync(long histId, CancellationToken cancellationToken = default)
        => await context.UserProfileHistories.FindAsync([histId], cancellationToken);

    public async Task<IEnumerable<UserProfileHistory>> GetByUserSysIdAsync(long userSysId, CancellationToken cancellationToken = default)
        => await context.UserProfileHistories
            .AsNoTracking()
            .Where(h => h.UserSysId == userSysId)
            .OrderByDescending(h => h.ChangedOn)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<UserProfileHistory>> GetByPolicyIdAsync(long policyId, CancellationToken cancellationToken = default)
        => await context.UserProfileHistories
            .AsNoTracking()
            .Where(h => h.PolicyId == policyId)
            .OrderByDescending(h => h.ChangedOn)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<UserProfileHistory>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        => await context.UserProfileHistories
            .AsNoTracking()
            .Where(h => h.ChangedOn >= from && h.ChangedOn <= to)
            .OrderByDescending(h => h.ChangedOn)
            .ToListAsync(cancellationToken);

    public async Task<UserProfileHistory> AddAsync(UserProfileHistory history, CancellationToken cancellationToken = default)
    {
        await context.UserProfileHistories.AddAsync(history, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return history;
    }
}
