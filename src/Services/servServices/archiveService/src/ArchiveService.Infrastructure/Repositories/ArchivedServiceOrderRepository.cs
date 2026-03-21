using ArchiveService.Domain.Entities;
using ArchiveService.Domain.Interfaces;
using ArchiveService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArchiveService.Infrastructure.Repositories;

public class ArchivedServiceOrderRepository(ArchiveDbContext context) : IArchivedServiceOrderRepository
{
    public async Task<ArchivedServiceOrder?> GetByIdAsync(string sernoDell, CancellationToken ct = default)
        => await context.ArchivedServiceOrders
            .Include(o => o.Details)
            .FirstOrDefaultAsync(o => o.SernoDell == sernoDell, ct);

    public async Task<ArchivedServiceOrder?> GetBySapIdAsync(string sapId, CancellationToken ct = default)
        => await context.ArchivedServiceOrders
            .Include(o => o.Details)
            .FirstOrDefaultAsync(o => o.SapId == sapId, ct);

    public async Task<IReadOnlyList<ArchivedServiceOrder>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        => await context.ArchivedServiceOrders
            .OrderByDescending(o => o.EnteredOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ArchivedServiceOrder>> SearchAsync(
        string? branch, string? engineerId, string? callStatus,
        DateTime? fromDate, DateTime? toDate, CancellationToken ct = default)
    {
        var query = context.ArchivedServiceOrders.AsQueryable();

        if (!string.IsNullOrEmpty(branch))
            query = query.Where(o => o.Branch == branch);
        if (!string.IsNullOrEmpty(callStatus))
            query = query.Where(o => o.CallStatus == callStatus);
        if (fromDate.HasValue)
            query = query.Where(o => o.PostingDate >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(o => o.PostingDate <= toDate.Value);

        return await query.OrderByDescending(o => o.EnteredOn).ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct = default)
        => await context.ArchivedServiceOrders.CountAsync(ct);

    public async Task AddAsync(ArchivedServiceOrder order, CancellationToken ct = default)
        => await context.ArchivedServiceOrders.AddAsync(order, ct);

    public Task UpdateAsync(ArchivedServiceOrder order, CancellationToken ct = default)
    {
        context.ArchivedServiceOrders.Update(order);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string sernoDell, CancellationToken ct = default)
    {
        var order = await context.ArchivedServiceOrders.FindAsync([sernoDell], ct);
        if (order is not null)
            context.ArchivedServiceOrders.Remove(order);
    }
}
