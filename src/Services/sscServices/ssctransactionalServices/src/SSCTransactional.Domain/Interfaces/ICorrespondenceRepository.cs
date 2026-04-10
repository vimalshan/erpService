using SSCTransactional.Domain.Aggregates;

namespace SSCTransactional.Domain.Interfaces;

public interface ICorrespondenceRepository
{
    Task<CorrespondenceAggregate?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<CorrespondenceAggregate>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<CorrespondenceAggregate>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task<IEnumerable<CorrespondenceAggregate>> GetActiveHoldsAsync(CancellationToken ct = default);
    Task AddAsync(CorrespondenceAggregate correspondence, CancellationToken ct = default);
    Task UpdateAsync(CorrespondenceAggregate correspondence, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}
