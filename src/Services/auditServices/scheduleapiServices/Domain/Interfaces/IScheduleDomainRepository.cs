using ScheduleService.Domain.Entities;

namespace ScheduleService.Domain.Interfaces;

public interface IScheduleDomainRepository
{
    Task<AuditSiteAudit?> GetByIdAsync(int id);
    Task<IEnumerable<AuditSiteAudit>> GetAllAsync();
    Task<IEnumerable<AuditSiteAudit>> GetByAuditAsync(int auditId);
    Task<IEnumerable<AuditSiteAudit>> GetBySiteAsync(int siteId);
    Task<AuditSiteAudit> AddAsync(AuditSiteAudit auditSiteAudit);
    Task UpdateAsync(AuditSiteAudit auditSiteAudit);
    Task DeleteAsync(int id);
}
