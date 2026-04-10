using Microsoft.EntityFrameworkCore;
using SSCTransactional.Domain.Aggregates;
using SSCTransactional.Domain.Interfaces;
using SSCTransactional.Infrastructure.Persistence;

namespace SSCTransactional.Infrastructure.Repositories;

public class CorrespondenceRepository : ICorrespondenceRepository
{
    private readonly ApplicationDbContext _context;

    public CorrespondenceRepository(ApplicationDbContext context) => _context = context;

    public async Task<CorrespondenceAggregate?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.Correspondences.Include(c => c.Attachments)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IEnumerable<CorrespondenceAggregate>> GetAllAsync(CancellationToken ct = default)
        => await _context.Correspondences.Include(c => c.Attachments).ToListAsync(ct);

    public async Task<IEnumerable<CorrespondenceAggregate>> GetByDocIdAsync(long docId, CancellationToken ct = default)
        => await _context.Correspondences.Include(c => c.Attachments)
            .Where(c => c.DocId == docId).ToListAsync(ct);

    public async Task<IEnumerable<CorrespondenceAggregate>> GetActiveHoldsAsync(CancellationToken ct = default)
        => await _context.Correspondences.Where(c => c.HoldStatus == "H").ToListAsync(ct);

    public async Task AddAsync(CorrespondenceAggregate correspondence, CancellationToken ct = default)
        => await _context.Correspondences.AddAsync(correspondence, ct);

    public Task UpdateAsync(CorrespondenceAggregate correspondence, CancellationToken ct = default)
    {
        _context.Correspondences.Update(correspondence);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.Correspondences.MaxAsync(c => (long?)c.Id, ct) ?? 0;
        return maxId + 1;
    }
}
