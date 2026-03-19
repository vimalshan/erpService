using Microsoft.EntityFrameworkCore;
using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Interfaces;
using BatchAndEnvelopeService.Infrastructure.Persistence;

namespace BatchAndEnvelopeService.Infrastructure.Repositories;

public class BatchRepository : IBatchRepository
{
    private readonly ApplicationDbContext _context;

    public BatchRepository(ApplicationDbContext context) => _context = context;

    public async Task<BatchAggregate?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.Batches
            .Include(b => b.Details)
            .Include(b => b.ReceiptDetails)
            .FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<IEnumerable<BatchAggregate>> GetAllAsync(CancellationToken ct = default)
        => await _context.Batches
            .Include(b => b.Details)
            .OrderByDescending(b => b.CreatedOn)
            .ToListAsync(ct);

    public async Task<IEnumerable<BatchAggregate>> GetByLocationAsync(long locationId, CancellationToken ct = default)
        => await _context.Batches
            .Where(b => b.LocationId == locationId)
            .Include(b => b.Details)
            .ToListAsync(ct);

    public async Task AddAsync(BatchAggregate batch, CancellationToken ct = default)
        => await _context.Batches.AddAsync(batch, ct);

    public Task UpdateAsync(BatchAggregate batch, CancellationToken ct = default)
    {
        _context.Batches.Update(batch);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.Batches.MaxAsync(b => (long?)b.Id, ct) ?? 0;
        return max + 1;
    }
}
