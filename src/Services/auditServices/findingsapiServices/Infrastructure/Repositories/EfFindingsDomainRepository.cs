using FindingsAPI.Gateway.Domain.Entities;
using FindingsAPI.Gateway.Domain.Interfaces;
using FindingsAPI.Gateway.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FindingsAPI.Gateway.Infrastructure.Repositories;

public class EfFindingsDomainRepository : IFindingsDomainRepository
{
    private readonly FindingsDomainDbContext _context;
    public EfFindingsDomainRepository(FindingsDomainDbContext context) => _context = context;

    public async Task<FindingEntity?> GetByIdAsync(int findingId) =>
        await _context.Findings
            .Include(f => f.FindingStatus)
            .Include(f => f.FindingCategory)
            .Include(f => f.Responses.Where(r => r.IsActive))
            .FirstOrDefaultAsync(f => f.FindingId == findingId);

    public async Task<IEnumerable<FindingEntity>> GetAllAsync() =>
        await _context.Findings
            .Include(f => f.FindingStatus)
            .Include(f => f.FindingCategory)
            .Where(f => f.IsActive)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();

    public async Task<IEnumerable<FindingEntity>> GetByAuditAsync(int auditId) =>
        await _context.Findings
            .Include(f => f.FindingStatus)
            .Where(f => f.AuditId == auditId && f.IsActive)
            .OrderByDescending(f => f.IdentifiedDate)
            .ToListAsync();

    public async Task<IEnumerable<FindingEntity>> GetBySiteAsync(int siteId) =>
        await _context.Findings
            .Include(f => f.FindingStatus)
            .Where(f => f.SiteId == siteId && f.IsActive)
            .OrderByDescending(f => f.IdentifiedDate)
            .ToListAsync();

    public async Task<FindingEntity> AddAsync(FindingEntity entity)
    {
        _context.Findings.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(FindingEntity entity)
    {
        _context.Findings.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int findingId)
    {
        var entity = await _context.Findings.FindAsync(findingId);
        if (entity != null)
        {
            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<FindingStatusEntity>> GetStatusesAsync() =>
        await _context.FindingStatuses.Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToListAsync();

    public async Task<IEnumerable<FindingCategoryEntity>> GetCategoriesAsync() =>
        await _context.FindingCategories.Where(c => c.IsActive).OrderBy(c => c.DisplayOrder).ToListAsync();

    public async Task<FindingResponseEntity> AddResponseAsync(FindingResponseEntity response)
    {
        _context.FindingResponses.Add(response);
        await _context.SaveChangesAsync();
        return response;
    }

    public async Task<IEnumerable<FindingResponseEntity>> GetResponsesByFindingAsync(int findingId) =>
        await _context.FindingResponses
            .Where(r => r.FindingId == findingId && r.IsActive)
            .OrderByDescending(r => r.ResponseDate)
            .ToListAsync();
}
