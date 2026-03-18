using AuditService.Domain.Entities;

namespace AuditService.Domain.Interfaces;

public interface IAuditRepository
{
    Task<AuditMaster?> GetByIdAsync(long auditId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditMaster>> GetByUnitAsync(long unitId, CancellationToken cancellationToken = default);
    Task AddAsync(AuditMaster audit, CancellationToken cancellationToken = default);
    Task UpdateAsync(AuditMaster audit, CancellationToken cancellationToken = default);
    Task DeleteAsync(long auditId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long auditId, CancellationToken cancellationToken = default);
}
