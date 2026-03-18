using RiskService.Domain.Aggregates;

namespace RiskService.Domain.Interfaces;

public interface IMitigationRepository
{
    Task<RiskMitigation?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<RiskMitigation>> GetByRiskIdAsync(long riskId, CancellationToken ct = default);
    Task AddAsync(RiskMitigation mitigation, CancellationToken ct = default);
    void Update(RiskMitigation mitigation);
}
