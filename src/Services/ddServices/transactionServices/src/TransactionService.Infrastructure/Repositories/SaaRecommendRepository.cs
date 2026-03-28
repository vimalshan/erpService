using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Repositories;

public class SaaRecommendRepository : ISaaRecommendRepository
{
    private readonly Data.TransactionDbContext _context;

    public SaaRecommendRepository(Data.TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<SaaRecommend?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.SaaRecommends
            .Include(r => r.Period)
            .Include(r => r.Level)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<SaaRecommend>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaaRecommends
            .Include(r => r.Period)
            .Include(r => r.Level)
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SaaRecommend>> GetByPeriodAsync(long periodId, CancellationToken cancellationToken = default)
    {
        return await _context.SaaRecommends
            .Include(r => r.Period)
            .Include(r => r.Level)
            .Where(a => a.PeriodId == periodId && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SaaRecommend>> GetByEmployeeAsync(long empSysId, CancellationToken cancellationToken = default)
    {
        return await _context.SaaRecommends
            .Include(r => r.Period)
            .Include(r => r.Level)
            .Where(a => a.EmpSysId == empSysId && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SaaRecommend>> GetByStatusAsync(long status, CancellationToken cancellationToken = default)
    {
        return await _context.SaaRecommends
            .Include(r => r.Period)
            .Include(r => r.Level)
            .Where(a => a.Status == status && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SaaRecommend>> GetPendingReviewAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaaRecommends
            .Include(r => r.Period)
            .Include(r => r.Level)
            .Where(a => a.RecommendSubmitBy != null && a.ReviewerSubmitBy == null && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<SaaRecommend> AddAsync(SaaRecommend entity, CancellationToken cancellationToken = default)
    {
        await _context.SaaRecommends.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(SaaRecommend entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SaaRecommends.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.SaaRecommends.Update(entity);
        }
    }
}
