using ArchiveService.Domain.Entities;
using ArchiveService.Domain.Interfaces;
using ArchiveService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArchiveService.Infrastructure.Repositories;

public class ArchivedToolKitRepository(ArchiveDbContext context) : IArchivedToolKitRepository
{
    public async Task<ArchivedToolKit?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.ArchivedToolKits
            .Include(t => t.Transactions)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<IReadOnlyList<ArchivedToolKit>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        => await context.ArchivedToolKits
            .OrderByDescending(t => t.EnteredOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ArchivedToolKit>> GetByEngineerIdAsync(string engineerId, CancellationToken ct = default)
        => await context.ArchivedToolKits
            .Where(t => t.EngineerId == engineerId)
            .ToListAsync(ct);

    public async Task AddAsync(ArchivedToolKit toolkit, CancellationToken ct = default)
        => await context.ArchivedToolKits.AddAsync(toolkit, ct);

    public Task UpdateAsync(ArchivedToolKit toolkit, CancellationToken ct = default)
    {
        context.ArchivedToolKits.Update(toolkit);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var toolkit = await context.ArchivedToolKits.FindAsync([id], ct);
        if (toolkit is not null)
            context.ArchivedToolKits.Remove(toolkit);
    }
}
