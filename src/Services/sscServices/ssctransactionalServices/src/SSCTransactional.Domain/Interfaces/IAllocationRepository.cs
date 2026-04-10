using SSCTransactional.Domain.Aggregates;

namespace SSCTransactional.Domain.Interfaces;

public interface IAllocationRepository
{
    Task<AllocationAggregate?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<AllocationAggregate>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<AllocationAggregate>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task<IEnumerable<AllocationAggregate>> GetByGroupIdAsync(long groupId, CancellationToken ct = default);
    Task<IEnumerable<AllocationAggregate>> GetPendingByGroupAsync(long groupId, CancellationToken ct = default);
    Task AddAsync(AllocationAggregate allocation, CancellationToken ct = default);
    Task UpdateAsync(AllocationAggregate allocation, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}
