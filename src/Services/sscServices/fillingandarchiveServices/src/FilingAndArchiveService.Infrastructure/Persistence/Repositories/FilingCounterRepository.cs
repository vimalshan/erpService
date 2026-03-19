using FilingAndArchiveService.Domain.Entities;
using FilingAndArchiveService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilingAndArchiveService.Infrastructure.Persistence.Repositories;

public class FilingCounterRepository : IFilingCounterRepository
{
    private readonly ApplicationDbContext _context;

    public FilingCounterRepository(ApplicationDbContext context) => _context = context;

    public Task<FilingCounter?> GetByBuIdAsync(string buId, CancellationToken cancellationToken = default)
        => _context.FilingCounters.FirstOrDefaultAsync(c => c.FilingBuId == buId, cancellationToken);

    public async Task<long> GetNextCountAsync(string buId, CancellationToken cancellationToken = default)
    {
        // Use a global counter so FILE_ID is unique across all orgs
        const string globalKey = "__GLOBAL__";
        var counter = await _context.FilingCounters
            .FirstOrDefaultAsync(c => c.FilingBuId == globalKey, cancellationToken);

        if (counter is null)
        {
            // Seed from current max FILE_ID to avoid conflicts with existing data
            var maxId = await _context.FileMasters.AnyAsync(cancellationToken)
                ? await _context.FileMasters.MaxAsync(f => f.FileId, cancellationToken)
                : 0L;
            counter = new FilingCounter { FilingBuId = globalKey, FileCount = maxId };
            await _context.FilingCounters.AddAsync(counter, cancellationToken);
        }

        return counter.NextCount();
    }

    public async Task UpsertAsync(FilingCounter counter, CancellationToken cancellationToken = default)
    {
        var existing = await _context.FilingCounters
            .FirstOrDefaultAsync(c => c.FilingBuId == counter.FilingBuId, cancellationToken);

        if (existing is null)
            await _context.FilingCounters.AddAsync(counter, cancellationToken);
        else
            existing.FileCount = counter.FileCount;
    }
}
