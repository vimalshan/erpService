using ArchiveService.Domain.Entities;
using ArchiveService.Domain.Interfaces;
using ArchiveService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArchiveService.Infrastructure.Repositories;

public class ArchivedToolKitTransactionRepository(ArchiveDbContext context) : IArchivedToolKitTransactionRepository
{
    public async Task<IReadOnlyList<ArchivedToolKitTransaction>> GetByToolkitIdAsync(long toolkitId, CancellationToken ct = default)
        => await context.ArchivedToolKitTransactions
            .Where(t => t.ToolkitId == toolkitId)
            .ToListAsync(ct);

    public async Task<ArchivedToolKitTransaction?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.ArchivedToolKitTransactions.FindAsync([id], ct);

    public async Task AddAsync(ArchivedToolKitTransaction transaction, CancellationToken ct = default)
        => await context.ArchivedToolKitTransactions.AddAsync(transaction, ct);
}
