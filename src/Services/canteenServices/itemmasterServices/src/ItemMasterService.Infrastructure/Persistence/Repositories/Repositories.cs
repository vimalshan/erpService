using Microsoft.EntityFrameworkCore;
using ItemMasterService.Domain.Entities;
using ItemMasterService.Domain.Interfaces;
using ItemMasterService.Infrastructure.Persistence.EF;

namespace ItemMasterService.Infrastructure.Persistence.Repositories;

public class CanteenItemRepository : ICanteenItemRepository
{
    private readonly ItemMasterDbContext _db;
    public CanteenItemRepository(ItemMasterDbContext db) => _db = db;

    public Task<CanteenItemMaster?> GetByIdAsync(long canteenUnitCode, long itemCode, CancellationToken ct = default) =>
        _db.CanteenItemMasters
           .Include(e => e.PriceMasters)
           .FirstOrDefaultAsync(e => e.CanteenUnitCode == canteenUnitCode && e.ItemCode == itemCode, ct);

    public async Task<IEnumerable<CanteenItemMaster>> GetAllAsync(long canteenUnitCode, CancellationToken ct = default) =>
        await _db.CanteenItemMasters
                 .Where(e => e.CanteenUnitCode == canteenUnitCode)
                 .AsNoTracking()
                 .ToListAsync(ct);

    public Task<bool> ExistsAsync(long canteenUnitCode, long itemCode, CancellationToken ct = default) =>
        _db.CanteenItemMasters.AnyAsync(e => e.CanteenUnitCode == canteenUnitCode && e.ItemCode == itemCode, ct);

    public async Task AddAsync(CanteenItemMaster entity, CancellationToken ct = default) =>
        await _db.CanteenItemMasters.AddAsync(entity, ct);

    public void Update(CanteenItemMaster entity) => _db.CanteenItemMasters.Update(entity);

    public void Delete(CanteenItemMaster entity) => _db.CanteenItemMasters.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}

public class CanteenItemPriceRepository : ICanteenItemPriceRepository
{
    private readonly ItemMasterDbContext _db;
    public CanteenItemPriceRepository(ItemMasterDbContext db) => _db = db;

    public Task<CanteenItemPriceMaster?> GetActiveAsync(long canteenUnitCode, long itemCode, CancellationToken ct = default) =>
        _db.CanteenItemPriceMasters
           .Where(e => e.CanteenUnitCode == canteenUnitCode && e.ItemCode == itemCode && e.ClosureDate == null)
           .OrderByDescending(e => e.EffectiveDate)
           .FirstOrDefaultAsync(ct);

    public async Task<IEnumerable<CanteenItemPriceMaster>> GetHistoryAsync(long canteenUnitCode, long itemCode, CancellationToken ct = default) =>
        await _db.CanteenItemPriceMasters
                 .Where(e => e.CanteenUnitCode == canteenUnitCode && e.ItemCode == itemCode)
                 .OrderByDescending(e => e.EffectiveDate)
                 .AsNoTracking()
                 .ToListAsync(ct);

    public async Task AddAsync(CanteenItemPriceMaster entity, CancellationToken ct = default) =>
        await _db.CanteenItemPriceMasters.AddAsync(entity, ct);

    public void Update(CanteenItemPriceMaster entity) => _db.CanteenItemPriceMasters.Update(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}

public class CanteenGradeItemPriceRepository : ICanteenGradeItemPriceRepository
{
    private readonly ItemMasterDbContext _db;
    public CanteenGradeItemPriceRepository(ItemMasterDbContext db) => _db = db;

    public Task<CanteenGradeItemPrice?> GetByUnitCodeAsync(long canteenUnitCode, CancellationToken ct = default) =>
        _db.CanteenGradeItemPrices.FirstOrDefaultAsync(e => e.CanteenUnitCode == canteenUnitCode, ct);

    public async Task<IEnumerable<CanteenGradeItemPrice>> GetAllAsync(CancellationToken ct = default) =>
        await _db.CanteenGradeItemPrices.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(CanteenGradeItemPrice entity, CancellationToken ct = default) =>
        await _db.CanteenGradeItemPrices.AddAsync(entity, ct);

    public void Update(CanteenGradeItemPrice entity) => _db.CanteenGradeItemPrices.Update(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
