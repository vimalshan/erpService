using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Repositories;

public sealed class ItemRepository : IItemRepository
{
    private readonly InventoryDbContext _ctx;

    public ItemRepository(InventoryDbContext ctx) => _ctx = ctx;

    public Task<ItemMaster?> GetByIdAsync(int itemId, CancellationToken ct = default)
        => _ctx.ItemMasters
               .Include(x => x.MainProduct)
               .Include(x => x.PackageType)
               .Include(x => x.UnitOfMeasure)
               .FirstOrDefaultAsync(x => x.SciItemId == itemId, ct);

    public Task<ItemMaster?> GetByOracleCodeAsync(string oracleCode, CancellationToken ct = default)
        => _ctx.ItemMasters
               .FirstOrDefaultAsync(x => x.OracleCode == oracleCode, ct);

    public async Task<IEnumerable<ItemMaster>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.ItemMasters
                     .Include(x => x.MainProduct)
                     .Include(x => x.UnitOfMeasure)
                     .ToListAsync(ct);

    public async Task<IEnumerable<ItemMaster>> GetByProductIdAsync(int productId, CancellationToken ct = default)
        => await _ctx.ItemMasters
                     .Where(x => x.MainProductId == productId)
                     .ToListAsync(ct);

    public async Task<ItemMaster> AddAsync(ItemMaster item, CancellationToken ct = default)
    {
        await _ctx.ItemMasters.AddAsync(item, ct);
        return item;
    }

    public Task UpdateAsync(ItemMaster item, CancellationToken ct = default)
    {
        _ctx.ItemMasters.Update(item);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int itemId, CancellationToken ct = default)
    {
        var entity = await _ctx.ItemMasters.FindAsync([itemId], ct);
        if (entity is not null) _ctx.ItemMasters.Remove(entity);
    }

    public Task<bool> OracleCodeExistsAsync(string oracleCode, CancellationToken ct = default)
        => _ctx.ItemMasters.AnyAsync(x => x.OracleCode == oracleCode, ct);
}
