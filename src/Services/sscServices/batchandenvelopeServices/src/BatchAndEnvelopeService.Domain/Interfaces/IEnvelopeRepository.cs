using BatchAndEnvelopeService.Domain.Aggregates;

namespace BatchAndEnvelopeService.Domain.Interfaces;

public interface IEnvelopeRepository
{
    Task<EnvelopeAggregate?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<EnvelopeAggregate>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<EnvelopeAggregate>> GetByTypeAsync(string envelopeType, CancellationToken ct = default);
    Task AddAsync(EnvelopeAggregate envelope, CancellationToken ct = default);
    Task UpdateAsync(EnvelopeAggregate envelope, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}
