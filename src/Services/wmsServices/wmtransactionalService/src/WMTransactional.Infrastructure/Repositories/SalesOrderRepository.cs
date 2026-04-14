using Microsoft.EntityFrameworkCore;
using WMTransactional.Domain.Entities;
using WMTransactional.Domain.Interfaces;
using WMTransactional.Infrastructure.Persistence;

namespace WMTransactional.Infrastructure.Repositories;

public class SalesOrderRepository : ISalesOrderRepository
{
    private readonly WMTransactionalDbContext _context;

    public SalesOrderRepository(WMTransactionalDbContext context)
    {
        _context = context;
    }

    public async Task<SalesOrder?> GetByIdAsync(int soId, CancellationToken ct = default)
    {
        return await _context.SalesOrders
            .Include(s => s.Lines)
            .FirstOrDefaultAsync(s => s.SoId == soId, ct);
    }

    public async Task<SalesOrder?> GetByNumberAsync(string soNumber, CancellationToken ct = default)
    {
        return await _context.SalesOrders
            .Include(s => s.Lines)
            .FirstOrDefaultAsync(s => s.SoNumber == soNumber, ct);
    }

    public async Task<IEnumerable<SalesOrder>> GetByCustomerAsync(int customerId, CancellationToken ct = default)
    {
        return await _context.SalesOrders
            .Include(s => s.Lines)
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<SalesOrder>> GetByStatusAsync(string status, CancellationToken ct = default)
    {
        return await _context.SalesOrders
            .Include(s => s.Lines)
            .Where(s => s.Status == status)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<SalesOrder>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.SalesOrders
            .Include(s => s.Lines)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task AddAsync(SalesOrder salesOrder, CancellationToken ct = default)
    {
        await _context.SalesOrders.AddAsync(salesOrder, ct);
    }

    public Task UpdateAsync(SalesOrder salesOrder, CancellationToken ct = default)
    {
        _context.SalesOrders.Update(salesOrder);
        return Task.CompletedTask;
    }
}
