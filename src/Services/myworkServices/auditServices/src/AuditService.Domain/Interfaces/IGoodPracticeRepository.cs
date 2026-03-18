using AuditService.Domain.Entities;

namespace AuditService.Domain.Interfaces;

public interface IGoodPracticeRepository
{
    Task<AuditGoodPractice?> GetByIdAsync(long practiceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditGoodPractice>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditGoodPractice>> GetByUnitAsync(long unitId, CancellationToken cancellationToken = default);
    Task AddAsync(AuditGoodPractice practice, CancellationToken cancellationToken = default);
    Task UpdateAsync(AuditGoodPractice practice, CancellationToken cancellationToken = default);
}
