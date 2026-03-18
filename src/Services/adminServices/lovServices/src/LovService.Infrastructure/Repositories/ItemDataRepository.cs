using LovService.Application.Interfaces;
using LovService.Domain.Entities;
using LovService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LovService.Infrastructure.Repositories;

public class ItemDataRepository(LovDbContext context) : IItemDataRepository
{
    public async Task<ItemData?> GetByIdAsync(int id, CancellationToken ct = default)
        => await context.ItemDataSet.FindAsync([id], ct);

    public async Task<IEnumerable<ItemData>> GetAllAsync(CancellationToken ct = default)
        => await context.ItemDataSet.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<ItemData>> SearchAsync(string? catName, string? itemName, CancellationToken ct = default)
    {
        var query = context.ItemDataSet.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(catName))
            query = query.Where(x => x.CatName != null && x.CatName.Contains(catName));
        if (!string.IsNullOrWhiteSpace(itemName))
            query = query.Where(x => x.ItemName != null && x.ItemName.Contains(itemName));
        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(ItemData itemData, CancellationToken ct = default)
        => await context.ItemDataSet.AddAsync(itemData, ct);

    public void Update(ItemData itemData)
        => context.ItemDataSet.Update(itemData);

    public void Delete(ItemData itemData)
        => context.ItemDataSet.Remove(itemData);
}
