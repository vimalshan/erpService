using AuditService.Domain.Entities;

namespace AuditService.Domain.Interfaces;

public interface IAuditDomainRepository
{
    Task<Audit?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Audit>> GetAllAsync(CancellationToken ct = default);
    Task<Audit> AddAsync(Audit entity, CancellationToken ct = default);
    Task UpdateAsync(Audit entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<AuditType>> GetAuditTypesAsync(CancellationToken ct = default);
    Task<IEnumerable<AuditSiteAudit>> GetSiteAuditsAsync(int auditId, CancellationToken ct = default);
}
