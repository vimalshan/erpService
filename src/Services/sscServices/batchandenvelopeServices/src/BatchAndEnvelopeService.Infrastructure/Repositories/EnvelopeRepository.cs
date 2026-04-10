using Microsoft.EntityFrameworkCore;
using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Interfaces;
using BatchAndEnvelopeService.Infrastructure.Persistence;

namespace BatchAndEnvelopeService.Infrastructure.Repositories;

public class EnvelopeRepository : IEnvelopeRepository
{
    private readonly ApplicationDbContext _context;

    public EnvelopeRepository(ApplicationDbContext context) => _context = context;

    public async Task<EnvelopeAggregate?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.Envelopes
            .Include(e => e.Details)
            .Include(e => e.ReceiptDetails)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IEnumerable<EnvelopeAggregate>> GetAllAsync(CancellationToken ct = default)
        => await _context.Envelopes
            .Include(e => e.Details)
            .OrderByDescending(e => e.CreatedOn)
            .ToListAsync(ct);

    public async Task<IEnumerable<EnvelopeAggregate>> GetByTypeAsync(string envelopeType, CancellationToken ct = default)
        => await _context.Envelopes
            .Where(e => e.EnvelopeType == envelopeType)
            .Include(e => e.Details)
            .ToListAsync(ct);

    public async Task AddAsync(EnvelopeAggregate envelope, CancellationToken ct = default)
        => await _context.Envelopes.AddAsync(envelope, ct);

    public Task UpdateAsync(EnvelopeAggregate envelope, CancellationToken ct = default)
    {
        _context.Envelopes.Update(envelope);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.Envelopes.MaxAsync(e => (long?)e.Id, ct) ?? 0;
        return max + 1;
    }

    public async Task<long> GetNextDetailIdAsync(CancellationToken ct = default)
    {
        var max = await _context.EnvelopeDetails.MaxAsync(d => (long?)d.Id, ct) ?? 0;
        return max + 1;
    }
}
