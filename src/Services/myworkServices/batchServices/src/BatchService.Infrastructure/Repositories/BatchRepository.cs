using Microsoft.EntityFrameworkCore;
using BatchService.Domain.Entities;
using BatchService.Domain.Interfaces;
using BatchService.Infrastructure.Persistence;

namespace BatchService.Infrastructure.Repositories;

/// <summary>EF Core-based repository for BatchMaster.</summary>
public sealed class BatchRepository : IBatchRepository
{
    private readonly BatchDbContext _context;

    public BatchRepository(BatchDbContext context) => _context = context;

    public async Task<BatchMaster?> GetByIdAsync(long batchId, CancellationToken ct) =>
        await _context.BatchMasters.FindAsync([batchId], ct);

    public async Task<IEnumerable<BatchMaster>> GetAllAsync(CancellationToken ct) =>
        await _context.BatchMasters.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<BatchMaster>> GetByMonthAsync(int monthNo, CancellationToken ct) =>
        await _context.BatchMasters
                      .AsNoTracking()
                      .Where(b => b.BatchMonthNo == monthNo)
                      .ToListAsync(ct);

    public async Task AddAsync(BatchMaster batch, CancellationToken ct)
    {
        await _context.BatchMasters.AddAsync(batch, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(BatchMaster batch, CancellationToken ct)
    {
        _context.BatchMasters.Update(batch);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long batchId, CancellationToken ct)
    {
        var batch = await GetByIdAsync(batchId, ct);
        if (batch is not null)
        {
            _context.BatchMasters.Remove(batch);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> ExistsAsync(long batchId, CancellationToken ct) =>
        await _context.BatchMasters.AnyAsync(b => b.BatchId == batchId, ct);
}
