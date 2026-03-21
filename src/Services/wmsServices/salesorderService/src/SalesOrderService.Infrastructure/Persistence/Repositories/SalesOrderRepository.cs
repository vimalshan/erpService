using Microsoft.EntityFrameworkCore;
using SalesOrderService.Domain.Entities;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Infrastructure.Persistence.Repositories;

public sealed class SalesOrderRepository(SalesOrderDbContext db) : ISalesOrderRepository
{
    public async Task<SalesOrder?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.SalesOrders
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<SalesOrder?> GetBySoNumberAsync(string soNumber, CancellationToken ct = default) =>
        await db.SalesOrders
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.SoNumber == soNumber, ct);

    public async Task<IEnumerable<SalesOrder>> GetAllAsync(CancellationToken ct = default) =>
        await db.SalesOrders
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<SalesOrder>> GetByCustomerIdAsync(int customerId, CancellationToken ct = default) =>
        await db.SalesOrders
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(ct);

    public async Task AddAsync(SalesOrder order, CancellationToken ct = default) =>
        await db.SalesOrders.AddAsync(order, ct);

    public void Update(SalesOrder order) => db.SalesOrders.Update(order);

    public void Delete(SalesOrder order) => db.SalesOrders.Remove(order);
}
