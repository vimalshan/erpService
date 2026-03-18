using RiskService.Domain.Aggregates;

namespace RiskService.Domain.Interfaces;

public interface IRiskRepository
{
    Task<RiskAggregate?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<RiskAggregate>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RiskAggregate>> GetByOrganizationAsync(long orgId, CancellationToken ct = default);
    Task AddAsync(RiskAggregate risk, CancellationToken ct = default);
    void Update(RiskAggregate risk);
    void Delete(RiskAggregate risk);
}
