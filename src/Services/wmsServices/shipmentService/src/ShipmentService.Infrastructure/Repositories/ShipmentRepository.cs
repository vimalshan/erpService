using Microsoft.EntityFrameworkCore;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Domain.Entities;
using ShipmentService.Domain.Enums;
using ShipmentService.Infrastructure.Data;

namespace ShipmentService.Infrastructure.Repositories;

public sealed class ShipmentRepository : IShipmentRepository
{
    private readonly ShipmentDbContext _context;

    public ShipmentRepository(ShipmentDbContext context) => _context = context;

    public async Task<Shipment?> GetByIdAsync(int shipmentId, CancellationToken cancellationToken = default) =>
        await _context.Shipments
            .Include(s => s.Lines)
            .Include(s => s.Packages)
            .Include(s => s.TrackingHistory)
            .Include(s => s.DeliveryAttempts)
            .FirstOrDefaultAsync(s => s.Id == shipmentId, cancellationToken);

    public async Task<Shipment?> GetByNumberAsync(string shipmentNumber, CancellationToken cancellationToken = default) =>
        await _context.Shipments
            .Include(s => s.Lines)
            .Include(s => s.Packages)
            .Include(s => s.TrackingHistory)
            .Include(s => s.DeliveryAttempts)
            .FirstOrDefaultAsync(s => s.ShipmentNumber == shipmentNumber.ToUpper(), cancellationToken);

    public async Task<IEnumerable<Shipment>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default) =>
        await _context.Shipments
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Shipment>> GetByStatusAsync(ShipmentStatus status, CancellationToken cancellationToken = default) =>
        await _context.Shipments
            .Where(s => s.Status == status)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Shipment>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default) =>
        await _context.Shipments
            .OrderByDescending(s => s.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default) =>
        await _context.Shipments.CountAsync(cancellationToken);

    public async Task<Shipment> AddAsync(Shipment shipment, CancellationToken cancellationToken = default)
    {
        await _context.Shipments.AddAsync(shipment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return shipment;
    }

    public async Task UpdateAsync(Shipment shipment, CancellationToken cancellationToken = default)
    {
        _context.Shipments.Update(shipment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string shipmentNumber, CancellationToken cancellationToken = default) =>
        await _context.Shipments.AnyAsync(s => s.ShipmentNumber == shipmentNumber.ToUpper(), cancellationToken);
}
