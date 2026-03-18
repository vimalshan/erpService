using AccountingService.Domain.Entities;

namespace AccountingService.Domain.Interfaces;

public interface IGlPostingRepository
{
    Task<GlPosting?> GetByIdAsync(long postingId, CancellationToken ct = default);
    Task<IEnumerable<GlPosting>> GetByAccountCodeAsync(string accountCode, CancellationToken ct = default);
    Task AddAsync(GlPosting entity, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
