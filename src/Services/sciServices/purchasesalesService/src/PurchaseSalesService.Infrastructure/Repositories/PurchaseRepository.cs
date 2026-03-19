using Microsoft.EntityFrameworkCore;
using PurchaseSalesService.Domain.Entities;
using PurchaseSalesService.Domain.Interfaces;
using PurchaseSalesService.Infrastructure.Data;

namespace PurchaseSalesService.Infrastructure.Repositories;

public sealed class PurchaseRepository : IPurchaseRepository
{
    private readonly ApplicationDbContext _db;

    public PurchaseRepository(ApplicationDbContext db) => _db = db;

    public async Task<PurchaseDetail?> GetByIdAsync(long serialNumber, CancellationToken ct = default)
        => await _db.PurchaseDetails.FindAsync(new object[] { serialNumber }, ct);

    public async Task<IEnumerable<PurchaseDetail>> GetAllAsync(CancellationToken ct = default)
        => await _db.PurchaseDetails.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<PurchaseDetail>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default)
        => await _db.PurchaseDetails.AsNoTracking()
            .Where(p => p.TrackingNumber == trackingNumber)
            .ToListAsync(ct);

    public async Task AddAsync(PurchaseDetail purchase, CancellationToken ct = default)
        => await _db.PurchaseDetails.AddAsync(purchase, ct);

    public Task UpdateAsync(PurchaseDetail purchase, CancellationToken ct = default)
    {
        _db.PurchaseDetails.Update(purchase);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
