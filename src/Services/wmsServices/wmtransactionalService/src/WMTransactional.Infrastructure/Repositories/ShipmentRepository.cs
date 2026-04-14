using Microsoft.EntityFrameworkCore;
using WMTransactional.Domain.Entities;
using WMTransactional.Domain.Interfaces;
using WMTransactional.Infrastructure.Persistence;

namespace WMTransactional.Infrastructure.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private readonly WMTransactionalDbContext _context;

    public ShipmentRepository(WMTransactionalDbContext context)
    {
        _context = context;
    }

    public async Task<Shipment?> GetByIdAsync(int shipmentId, CancellationToken ct = default)
    {
        return await _context.Shipments
            .Include(s => s.Lines)
            .FirstOrDefaultAsync(s => s.ShipmentId == shipmentId, ct);
    }

    public async Task<Shipment?> GetByNumberAsync(string shipmentNumber, CancellationToken ct = default)
    {
        return await _context.Shipments
            .Include(s => s.Lines)
            .FirstOrDefaultAsync(s => s.ShipmentNumber == shipmentNumber, ct);
    }

    public async Task<IEnumerable<Shipment>> GetBySalesOrderAsync(int soId, CancellationToken ct = default)
    {
        return await _context.Shipments
            .Include(s => s.Lines)
            .Where(s => s.SoId == soId)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Shipment>> GetByStatusAsync(string status, CancellationToken ct = default)
    {
        return await _context.Shipments
            .Include(s => s.Lines)
            .Where(s => s.Status == status)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Shipment>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Shipments
            .Include(s => s.Lines)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Shipment shipment, CancellationToken ct = default)
    {
        await _context.Shipments.AddAsync(shipment, ct);
    }

    public Task UpdateAsync(Shipment shipment, CancellationToken ct = default)
    {
        _context.Shipments.Update(shipment);
        return Task.CompletedTask;
    }
}
