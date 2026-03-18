using Microsoft.EntityFrameworkCore;
using UserSecurityService.Domain.Entities;
using UserSecurityService.Domain.Interfaces;
using UserSecurityService.Infrastructure.Persistence;

namespace UserSecurityService.Infrastructure.Repositories;

public class UserProfileRepository(UserSecurityDbContext context) : IUserProfileRepository
{
    public async Task<UserProfilePfs?> GetByIdAsync(string userId, CancellationToken ct)
        => await context.UserProfiles.FirstOrDefaultAsync(u => u.EmUsrId == userId, ct);

    public async Task<UserProfilePfs?> GetByEmpNumAsync(decimal empNum, CancellationToken ct)
        => await context.UserProfiles.FirstOrDefaultAsync(u => u.EmEmpNum == empNum, ct);

    public async Task<IEnumerable<UserProfilePfs>> GetAllActiveAsync(CancellationToken ct)
        => await context.UserProfiles
            .Where(u => u.EmClsDat == null && u.EmRegStatus == "A")
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task AddAsync(UserProfilePfs profile, CancellationToken ct)
        => await context.UserProfiles.AddAsync(profile, ct);

    public void Update(UserProfilePfs profile)
        => context.UserProfiles.Update(profile);

    public void Remove(UserProfilePfs profile)
        => context.UserProfiles.Remove(profile);
}

public class UserAppsMappingRepository(UserSecurityDbContext context) : IUserAppsMappingRepository
{
    public async Task<UserAppsMap?> GetByEmpSysIdAsync(decimal empSysId, CancellationToken ct)
        => await context.UserAppsMappings.FirstOrDefaultAsync(x => x.UserEmpSysId == empSysId, ct);

    public async Task<IEnumerable<UserAppsMap>> GetByAppCodeAsync(string appCode, CancellationToken ct)
        => await context.UserAppsMappings
            .Where(x => x.UserApps == appCode)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task AddAsync(UserAppsMap entity, CancellationToken ct)
        => await context.UserAppsMappings.AddAsync(entity, ct);

    public void Update(UserAppsMap entity)
        => context.UserAppsMappings.Update(entity);
}

public class EmpPasswordChangeRepository(UserSecurityDbContext context) : IEmpPasswordChangeRepository
{
    public async Task AddAsync(EmpPasswordChange record, CancellationToken ct)
        => await context.EmpPasswordChanges.AddAsync(record, ct);

    public async Task<IEnumerable<EmpPasswordChange>> GetHistoryAsync(decimal empSysId, CancellationToken ct)
        => await context.EmpPasswordChanges
            .Where(x => x.EpwdEmpSysId == empSysId)
            .OrderByDescending(x => x.EpwdCreatedOn)
            .AsNoTracking()
            .ToListAsync(ct);
}
