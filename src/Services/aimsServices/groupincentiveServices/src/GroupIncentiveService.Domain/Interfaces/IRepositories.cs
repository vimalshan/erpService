using GroupIncentiveService.Domain.Entities;

namespace GroupIncentiveService.Domain.Interfaces;

public interface IGroupMasterRepository
{
    Task<GroupMaster?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<GroupMaster?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<IEnumerable<GroupMaster>> GetAllAsync(bool activeOnly = true, CancellationToken ct = default);
    Task<GroupMaster> AddAsync(GroupMaster group, CancellationToken ct = default);
    Task UpdateAsync(GroupMaster group, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
}

public interface IGroupEmployeeMapRepository
{
    Task<GroupEmployeeMap?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<GroupEmployeeMap>> GetByGroupIdAsync(int groupId, CancellationToken ct = default);
    Task<IEnumerable<GroupEmployeeMap>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default);
    Task<GroupEmployeeMap> AddAsync(GroupEmployeeMap mapping, CancellationToken ct = default);
    Task UpdateAsync(GroupEmployeeMap mapping, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IGroupIncentiveMainRepository
{
    Task<GroupIncentiveMain?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<GroupIncentiveMain?> GetByIdWithDetailsAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<GroupIncentiveMain>> GetByGroupIdAsync(int groupId, CancellationToken ct = default);
    Task<IEnumerable<GroupIncentiveMain>> GetPendingAsync(CancellationToken ct = default);
    Task<GroupIncentiveMain> AddAsync(GroupIncentiveMain incentive, CancellationToken ct = default);
    Task UpdateAsync(GroupIncentiveMain incentive, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IGroupIncentiveDetRepository
{
    Task<GroupIncentiveDet?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<GroupIncentiveDet>> GetByMainIdAsync(long mainId, CancellationToken ct = default);
    Task<GroupIncentiveDet> AddAsync(GroupIncentiveDet detail, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<GroupIncentiveDet> details, CancellationToken ct = default);
    Task UpdateAsync(GroupIncentiveDet detail, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IGroupIncentiveBreakRepository
{
    Task<GroupIncentiveBreak?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<GroupIncentiveBreak>> GetByGroupIdAsync(int groupId, CancellationToken ct = default);
    Task<GroupIncentiveBreak> AddAsync(GroupIncentiveBreak breakRule, CancellationToken ct = default);
    Task UpdateAsync(GroupIncentiveBreak breakRule, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
}

public interface IGroupIncentiveApprovalRepository
{
    Task<IEnumerable<GroupIncentiveApproval>> GetByMainIdAsync(long mainId, CancellationToken ct = default);
    Task<GroupIncentiveApproval> AddAsync(GroupIncentiveApproval approval, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}
