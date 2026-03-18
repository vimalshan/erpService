using AccountingService.Domain.Entities;
using AccountingService.Domain.Interfaces;
using AccountingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Infrastructure.Repositories;

public class GlPostingRepository : IGlPostingRepository
{
    private readonly AccountingDbContext _context;

    public GlPostingRepository(AccountingDbContext context)
        => _context = context;

    public async Task<GlPosting?> GetByIdAsync(long postingId, CancellationToken ct = default)
        => await _context.GlPostings.FindAsync([postingId], ct);

    public async Task<IEnumerable<GlPosting>> GetByAccountCodeAsync(string accountCode, CancellationToken ct = default)
        => await _context.GlPostings.Where(g => g.AccountCode == accountCode).ToListAsync(ct);

    public async Task AddAsync(GlPosting entity, CancellationToken ct = default)
        => await _context.GlPostings.AddAsync(entity, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
