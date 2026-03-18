using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Repositories;

public sealed class UnitOfMeasureRepository : IUnitOfMeasureRepository
{
    private readonly InventoryDbContext _ctx;

    public UnitOfMeasureRepository(InventoryDbContext ctx) => _ctx = ctx;

    public Task<UnitOfMeasure?> GetByIdAsync(int unitId, CancellationToken ct = default)
        => _ctx.UnitsOfMeasure.FirstOrDefaultAsync(x => x.UnitId == unitId, ct);

    public async Task<IEnumerable<UnitOfMeasure>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.UnitsOfMeasure.ToListAsync(ct);
}
