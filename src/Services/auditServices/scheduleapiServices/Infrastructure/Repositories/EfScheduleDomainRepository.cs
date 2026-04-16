using ScheduleService.Domain.Entities;
using ScheduleService.Domain.Interfaces;
using ScheduleService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ScheduleService.Infrastructure.Repositories;

public class EfScheduleDomainRepository : IScheduleDomainRepository
{
    private readonly ScheduleDomainDbContext _ctx;
    public EfScheduleDomainRepository(ScheduleDomainDbContext ctx) { _ctx = ctx; }

    public async Task<AuditSiteAudit?> GetByIdAsync(int id) =>
        await _ctx.AuditSiteAudits.FirstOrDefaultAsync(a => a.AuditSiteAuditId == id);

    public async Task<IEnumerable<AuditSiteAudit>> GetAllAsync() =>
        await _ctx.AuditSiteAudits.OrderByDescending(a => a.ScheduledDate).ToListAsync();

    public async Task<IEnumerable<AuditSiteAudit>> GetByAuditAsync(int auditId) =>
        await _ctx.AuditSiteAudits.Where(a => a.AuditId == auditId).OrderByDescending(a => a.ScheduledDate).ToListAsync();

    public async Task<IEnumerable<AuditSiteAudit>> GetBySiteAsync(int siteId) =>
        await _ctx.AuditSiteAudits.Where(a => a.SiteId == siteId).OrderByDescending(a => a.ScheduledDate).ToListAsync();

    public async Task<AuditSiteAudit> AddAsync(AuditSiteAudit auditSiteAudit)
    {
        _ctx.AuditSiteAudits.Add(auditSiteAudit); await _ctx.SaveChangesAsync(); return auditSiteAudit;
    }

    public async Task UpdateAsync(AuditSiteAudit auditSiteAudit)
    {
        _ctx.AuditSiteAudits.Update(auditSiteAudit); await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _ctx.AuditSiteAudits.FindAsync(id);
        if (entity != null) { _ctx.AuditSiteAudits.Remove(entity); await _ctx.SaveChangesAsync(); }
    }
}
