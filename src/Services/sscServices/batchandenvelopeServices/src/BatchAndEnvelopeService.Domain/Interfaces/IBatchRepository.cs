using BatchAndEnvelopeService.Domain.Aggregates;

namespace BatchAndEnvelopeService.Domain.Interfaces;

public interface IBatchRepository
{
    Task<BatchAggregate?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<BatchAggregate>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<BatchAggregate>> GetByLocationAsync(long locationId, CancellationToken ct = default);
    Task AddAsync(BatchAggregate batch, CancellationToken ct = default);
    Task UpdateAsync(BatchAggregate batch, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
    Task<int> GetNextDetailIdAsync(CancellationToken ct = default);
}
