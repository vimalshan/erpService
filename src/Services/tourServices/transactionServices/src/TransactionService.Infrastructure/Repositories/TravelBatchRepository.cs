using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Interfaces;
using TransactionService.Domain.ValueObjects;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Infrastructure.Repositories;

public sealed class TravelBatchRepository : ITravelBatchRepository
{
    private readonly TransactionDbContext _context;

    public TravelBatchRepository(TransactionDbContext context) => _context = context;

    public async Task<TravelBatch?> GetByIdAsync(string batchId, CancellationToken cancellationToken = default)
        => await _context.TravelBatches
            .Include(b => b.SubItems)
            .FirstOrDefaultAsync(b => b.BatchId == batchId, cancellationToken);

    public async Task<IEnumerable<TravelBatch>> GetByVendorIdAsync(string vendorId, CancellationToken cancellationToken = default)
        => await _context.TravelBatches
            .Include(b => b.SubItems)
            .Where(b => b.SubItems.Any(s => s.VendorId == vendorId))
            .OrderByDescending(b => b.BatchDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<TravelBatch>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
        => await _context.TravelBatches
            .Include(b => b.SubItems)
            .Where(b => b.Status == status)
            .OrderByDescending(b => b.BatchDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<TravelBatch>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _context.TravelBatches
            .Include(b => b.SubItems)
            .OrderByDescending(b => b.BatchDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        => await _context.TravelBatches.CountAsync(cancellationToken);

    public async Task AddAsync(TravelBatch batch, CancellationToken cancellationToken = default)
        => await _context.TravelBatches.AddAsync(batch, cancellationToken);

    public void Update(TravelBatch batch)
        => _context.TravelBatches.Update(batch);

    public async Task<bool> ExistsAsync(string batchId, CancellationToken cancellationToken = default)
        => await _context.TravelBatches.AnyAsync(b => b.BatchId == batchId, cancellationToken);
}
