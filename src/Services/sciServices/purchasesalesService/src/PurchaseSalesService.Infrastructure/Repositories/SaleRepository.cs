using Microsoft.EntityFrameworkCore;
using PurchaseSalesService.Domain.Entities;
using PurchaseSalesService.Domain.Interfaces;
using PurchaseSalesService.Infrastructure.Data;

namespace PurchaseSalesService.Infrastructure.Repositories;

public sealed class SaleRepository : ISaleRepository
{
    private readonly ApplicationDbContext _db;

    public SaleRepository(ApplicationDbContext db) => _db = db;

    public async Task<SaleMain?> GetByIdAsync(long serialNumber, CancellationToken ct = default)
        => await _db.SaleMains
            .Include(s => s.SaleSubItems)
            .FirstOrDefaultAsync(s => s.SerialNumber == serialNumber, ct);

    public async Task<IEnumerable<SaleMain>> GetAllAsync(CancellationToken ct = default)
        => await _db.SaleMains.AsNoTracking()
            .Include(s => s.SaleSubItems)
            .ToListAsync(ct);

    public async Task<IEnumerable<SaleMain>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default)
        => await _db.SaleMains.AsNoTracking()
            .Include(s => s.SaleSubItems)
            .Where(s => s.TrackingNumber == trackingNumber)
            .ToListAsync(ct);

    public async Task AddAsync(SaleMain sale, CancellationToken ct = default)
        => await _db.SaleMains.AddAsync(sale, ct);

    public Task UpdateAsync(SaleMain sale, CancellationToken ct = default)
    {
        _db.SaleMains.Update(sale);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
