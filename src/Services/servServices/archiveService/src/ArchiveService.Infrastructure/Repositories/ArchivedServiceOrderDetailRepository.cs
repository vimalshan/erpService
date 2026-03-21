using ArchiveService.Domain.Entities;
using ArchiveService.Domain.Interfaces;
using ArchiveService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArchiveService.Infrastructure.Repositories;

public class ArchivedServiceOrderDetailRepository(ArchiveDbContext context) : IArchivedServiceOrderDetailRepository
{
    public async Task<IReadOnlyList<ArchivedServiceOrderDetail>> GetByServiceOrderAsync(string sernoDell, CancellationToken ct = default)
        => await context.ArchivedServiceOrderDetails
            .Where(d => d.SernoDell == sernoDell)
            .ToListAsync(ct);

    public async Task<ArchivedServiceOrderDetail?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.ArchivedServiceOrderDetails.FindAsync([id], ct);

    public async Task AddAsync(ArchivedServiceOrderDetail detail, CancellationToken ct = default)
        => await context.ArchivedServiceOrderDetails.AddAsync(detail, ct);

    public Task UpdateAsync(ArchivedServiceOrderDetail detail, CancellationToken ct = default)
    {
        context.ArchivedServiceOrderDetails.Update(detail);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var detail = await context.ArchivedServiceOrderDetails.FindAsync([id], ct);
        if (detail is not null)
            context.ArchivedServiceOrderDetails.Remove(detail);
    }
}
