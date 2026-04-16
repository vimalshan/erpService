using AuditService.Domain.Entities;
using AuditService.Domain.Interfaces;
using AuditService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Infrastructure.Repositories;

public class EfAuditDomainRepository : IAuditDomainRepository
{
    private readonly AuditDomainDbContext _context;
    public EfAuditDomainRepository(AuditDomainDbContext context) => _context = context;

    public async Task<Audit?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Audits.Include(a => a.AuditSites).Include(a => a.AuditServices)
            .Include(a => a.AuditTeamMembers).FirstOrDefaultAsync(a => a.AuditId == id, ct);

    public async Task<IEnumerable<Audit>> GetAllAsync(CancellationToken ct = default)
        => await _context.Audits.ToListAsync(ct);

    public async Task<Audit> AddAsync(Audit entity, CancellationToken ct = default)
    {
        _context.Audits.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Audit entity, CancellationToken ct = default)
    {
        _context.Audits.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.Audits.FindAsync(new object[] { id }, ct);
        if (entity is not null) { _context.Audits.Remove(entity); await _context.SaveChangesAsync(ct); }
    }

    public async Task<IEnumerable<AuditType>> GetAuditTypesAsync(CancellationToken ct = default)
        => await _context.AuditTypes.Where(t => t.IsActive).ToListAsync(ct);

    public async Task<IEnumerable<AuditSiteAudit>> GetSiteAuditsAsync(int auditId, CancellationToken ct = default)
        => await _context.AuditSiteAudits
            .Include(s => s.Representatives).Include(s => s.SiteServices)
            .Where(s => s.AuditId == auditId).ToListAsync(ct);
}
