using UserSecurityService.Domain.Entities;

namespace UserSecurityService.Domain.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfilePfs?> GetByIdAsync(string userId, CancellationToken ct = default);
    Task<UserProfilePfs?> GetByEmpNumAsync(decimal empNum, CancellationToken ct = default);
    Task<IEnumerable<UserProfilePfs>> GetAllActiveAsync(CancellationToken ct = default);
    Task AddAsync(UserProfilePfs profile, CancellationToken ct = default);
    void Update(UserProfilePfs profile);
    void Remove(UserProfilePfs profile);
}

public interface IUserAppsMappingRepository
{
    Task<UserAppsMap?> GetByEmpSysIdAsync(decimal empSysId, CancellationToken ct = default);
    Task<IEnumerable<UserAppsMap>> GetByAppCodeAsync(string appCode, CancellationToken ct = default);
    Task AddAsync(UserAppsMap entity, CancellationToken ct = default);
    void Update(UserAppsMap entity);
}

public interface IEmpPasswordChangeRepository
{
    Task AddAsync(EmpPasswordChange record, CancellationToken ct = default);
    Task<IEnumerable<EmpPasswordChange>> GetHistoryAsync(decimal empSysId, CancellationToken ct = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
