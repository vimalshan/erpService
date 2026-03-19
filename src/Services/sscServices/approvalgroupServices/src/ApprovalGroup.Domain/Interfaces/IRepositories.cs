using ApprovalGroup.Domain.Entities;

namespace ApprovalGroup.Domain.Interfaces;

public interface IApprovalGroupRepository
{
    Task<ApprovalGroupMaster?> GetByIdAsync(long groupId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApprovalGroupMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApprovalGroupMaster> AddAsync(ApprovalGroupMaster group, CancellationToken cancellationToken = default);
    Task UpdateAsync(ApprovalGroupMaster group, CancellationToken cancellationToken = default);
    Task DeleteAsync(long groupId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long groupId, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
}

public interface IApprovalGroupMapRepository
{
    Task<ApprovalGroupMap?> GetByIdAsync(long mapId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApprovalGroupMap>> GetByGroupIdAsync(long groupId, CancellationToken cancellationToken = default);
    Task<ApprovalGroupMap> AddAsync(ApprovalGroupMap map, CancellationToken cancellationToken = default);
    Task UpdateAsync(ApprovalGroupMap map, CancellationToken cancellationToken = default);
    Task DeleteAsync(long mapId, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
}

public interface IApprovalGroupUserMapRepository
{
    Task<ApprovalGroupUserMap?> GetByIdAsync(long mapId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApprovalGroupUserMap>> GetByGroupIdAsync(long groupId, CancellationToken cancellationToken = default);
    Task<ApprovalGroupUserMap> AddAsync(ApprovalGroupUserMap userMap, CancellationToken cancellationToken = default);
    Task UpdateAsync(ApprovalGroupUserMap userMap, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
}

public interface IPullMatrixRepository
{
    Task<PullMatrixDetail?> GetByIdAsync(long matId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PullMatrixDetail>> GetByUnitIdAsync(long unitId, CancellationToken cancellationToken = default);
    Task<PullMatrixDetail> AddAsync(PullMatrixDetail detail, CancellationToken cancellationToken = default);
    Task UpdateAsync(PullMatrixDetail detail, CancellationToken cancellationToken = default);
    Task DeleteAsync(long matId, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
