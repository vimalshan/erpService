namespace TransactionService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

public sealed class OrderRepository : Repository<OrderMain>, IOrderRepository
{
    public OrderRepository(TransactionDbContext context) : base(context) { }

    public async Task<OrderMain?> GetByIdWithDetailsAsync(long orderMainId, CancellationToken ct = default)
    {
        return await _context.OrderMains
            .Include(o => o.Details)
            .FirstOrDefaultAsync(o => o.OrderMainId == orderMainId, ct);
    }

    public async Task<IEnumerable<OrderMain>> GetByLocationAsync(long locationId, CancellationToken ct = default)
    {
        return await _context.OrderMains
            .Include(o => o.Details)
            .Where(o => o.LocationId == locationId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<OrderMain>> GetByVendorAsync(long vendorId, CancellationToken ct = default)
    {
        return await _context.OrderMains
            .Include(o => o.Details)
            .Where(o => o.VendorId == vendorId)
            .ToListAsync(ct);
    }

    public async Task<long> GetNextOrderMainIdAsync(CancellationToken ct = default)
    {
        var max = await _context.OrderMains.MaxAsync(o => (long?)o.OrderMainId, ct);
        return (max ?? 0) + 1;
    }

    public async Task<long> GetNextOrderSubIdAsync(CancellationToken ct = default)
    {
        var max = await _context.OrderSubs.MaxAsync(o => (long?)o.OrderSubId, ct);
        return (max ?? 0) + 1;
    }
}
