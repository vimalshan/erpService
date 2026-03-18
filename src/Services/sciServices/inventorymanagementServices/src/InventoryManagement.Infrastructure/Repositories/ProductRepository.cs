using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _ctx;

    public ProductRepository(InventoryDbContext ctx) => _ctx = ctx;

    public Task<MainProductMaster?> GetByIdAsync(int productId, CancellationToken ct = default)
        => _ctx.MainProductMasters
               .Include(x => x.ProductType)
               .Include(x => x.Unit)
               .FirstOrDefaultAsync(x => x.ProductId == productId, ct);

    public async Task<IEnumerable<MainProductMaster>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.MainProductMasters
                     .Include(x => x.ProductType)
                     .Include(x => x.Unit)
                     .ToListAsync(ct);

    public async Task<MainProductMaster> AddAsync(MainProductMaster product, CancellationToken ct = default)
    {
        await _ctx.MainProductMasters.AddAsync(product, ct);
        return product;
    }

    public Task UpdateAsync(MainProductMaster product, CancellationToken ct = default)
    {
        _ctx.MainProductMasters.Update(product);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int productId, CancellationToken ct = default)
    {
        var entity = await _ctx.MainProductMasters.FindAsync([productId], ct);
        if (entity is not null) _ctx.MainProductMasters.Remove(entity);
    }

    public Task<bool> ExistsAsync(int productId, CancellationToken ct = default)
        => _ctx.MainProductMasters.AnyAsync(x => x.ProductId == productId, ct);
}
