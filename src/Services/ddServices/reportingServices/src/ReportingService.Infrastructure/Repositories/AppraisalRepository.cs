using Microsoft.EntityFrameworkCore;
using ReportingService.Domain.Entities;
using ReportingService.Domain.Interfaces;

namespace ReportingService.Infrastructure.Repositories;

public class AppraisalRepository : IAppraisalRepository
{
    private readonly Data.ReportingDbContext _context;

    public AppraisalRepository(Data.ReportingDbContext context)
    {
        _context = context;
    }

    public async Task<Appraisal?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Appraisals
            .Include(a => a.Goals)
            .Include(a => a.Performances)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<Appraisal?> GetByRequestNumberAsync(long requestNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Appraisals
            .Include(a => a.Goals)
            .Include(a => a.Performances)
            .FirstOrDefaultAsync(a => a.RequestNumber == requestNumber && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Appraisal>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Appraisals
            .Where(a => !a.IsDeleted)
            .Include(a => a.Goals)
            .Include(a => a.Performances)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appraisal>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Appraisals
            .Where(a => a.UserId == userId && !a.IsDeleted)
            .Include(a => a.Goals)
            .Include(a => a.Performances)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Appraisal entity, CancellationToken cancellationToken = default)
    {
        await _context.Appraisals.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(Appraisal entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Appraisals.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.Appraisals.Update(entity);
        }
    }
}
