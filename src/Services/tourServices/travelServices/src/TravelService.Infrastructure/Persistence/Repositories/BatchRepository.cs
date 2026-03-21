using Microsoft.EntityFrameworkCore;
using TravelService.Domain.Entities.Batch;
using TravelService.Domain.Repositories;
using TravelService.Infrastructure.Persistence;

namespace TravelService.Infrastructure.Persistence.Repositories;

public class BatchRepository : IBatchRepository
{
    private readonly TravelDbContext _context;

    public BatchRepository(TravelDbContext context) => _context = context;

    public async Task<BatchMain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        => await _context.BatchMains
            .Include(b => b.BatchSubs)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<IEnumerable<BatchMain>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _context.BatchMains
            .OrderByDescending(b => b.BatchDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<BatchMain> AddAsync(BatchMain batch, CancellationToken cancellationToken = default)
    {
        await _context.BatchMains.AddAsync(batch, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return batch;
    }

    public async Task UpdateAsync(BatchMain batch, CancellationToken cancellationToken = default)
    {
        _context.BatchMains.Update(batch);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
