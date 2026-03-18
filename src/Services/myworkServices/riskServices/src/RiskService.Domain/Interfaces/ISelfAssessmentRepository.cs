using RiskService.Domain.Aggregates;

namespace RiskService.Domain.Interfaces;

public interface ISelfAssessmentRepository
{
    Task<RiskSelfAssessment?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<RiskSelfAssessment>> GetPendingAsync(CancellationToken ct = default);
    Task AddAsync(RiskSelfAssessment assessment, CancellationToken ct = default);
    void Update(RiskSelfAssessment assessment);
}
