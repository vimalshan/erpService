using MamAllocationService.Domain.Entities;
using MamAllocationService.Domain.Interfaces;
using MamAllocationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MamAllocationService.Infrastructure.Repositories;

public class AllocationDetailRepository(MamAllocationDbContext db) : IAllocationDetailRepository
{
    public async Task<AllocationDetail?> GetByIdAsync(DateTime allocationDate, int rawMaterialCode, CancellationToken ct = default)
        => await db.AllocationDetails.FirstOrDefaultAsync(x => x.AllDate == allocationDate && x.AllRm == rawMaterialCode, ct);

    public async Task<IEnumerable<AllocationDetail>> GetByDateAsync(DateTime allocationDate, CancellationToken ct = default)
        => await db.AllocationDetails.Where(x => x.AllDate == allocationDate).ToListAsync(ct);

    public async Task<IEnumerable<AllocationDetail>> GetAllAsync(CancellationToken ct = default)
        => await db.AllocationDetails.ToListAsync(ct);

    public async Task AddAsync(AllocationDetail entity, CancellationToken ct = default)
        => await db.AllocationDetails.AddAsync(entity, ct);

    public void Update(AllocationDetail entity) => db.AllocationDetails.Update(entity);
    public void Delete(AllocationDetail entity) => db.AllocationDetails.Remove(entity);
}

public class AllocationProdDetailRepository(MamAllocationDbContext db) : IAllocationProdDetailRepository
{
    public async Task<IEnumerable<AllocationProdDetail>> GetByDateAsync(DateTime allocationDate, CancellationToken ct = default)
        => await db.AllocationProdDetails.Where(x => x.AllDate == allocationDate).ToListAsync(ct);

    public async Task<IEnumerable<AllocationProdDetail>> GetAllAsync(CancellationToken ct = default)
        => await db.AllocationProdDetails.ToListAsync(ct);

    public async Task AddAsync(AllocationProdDetail entity, CancellationToken ct = default)
        => await db.AllocationProdDetails.AddAsync(entity, ct);

    public void Delete(AllocationProdDetail entity) => db.AllocationProdDetails.Remove(entity);
}

public class AllocationFgRepository(MamAllocationDbContext db) : IAllocationFgRepository
{
    public async Task<IEnumerable<AllocationFg>> GetByDateAsync(DateTime allocationDate, CancellationToken ct = default)
        => await db.AllocationFgs.Where(x => x.AllDate == allocationDate).ToListAsync(ct);

    public async Task<IEnumerable<AllocationFg>> GetAllAsync(CancellationToken ct = default)
        => await db.AllocationFgs.ToListAsync(ct);

    public async Task AddAsync(AllocationFg entity, CancellationToken ct = default)
        => await db.AllocationFgs.AddAsync(entity, ct);

    public void Delete(AllocationFg entity) => db.AllocationFgs.Remove(entity);
}

public class ArrivalDetailRepository(MamAllocationDbContext db) : IArrivalDetailRepository
{
    public async Task<IEnumerable<ArrivalDetail>> GetByItemAsync(int arrivalItem, CancellationToken ct = default)
        => await db.ArrivalDetails.Where(x => x.ArrivalItem == arrivalItem).ToListAsync(ct);

    public async Task<IEnumerable<ArrivalDetail>> GetAllAsync(CancellationToken ct = default)
        => await db.ArrivalDetails.ToListAsync(ct);

    public async Task AddAsync(ArrivalDetail entity, CancellationToken ct = default)
        => await db.ArrivalDetails.AddAsync(entity, ct);

    public void Delete(ArrivalDetail entity) => db.ArrivalDetails.Remove(entity);
}

public class ConsumptionDetailRepository(MamAllocationDbContext db) : IConsumptionDetailRepository
{
    public async Task<IEnumerable<ConsumptionDetail>> GetByRmAsync(int consumptionRm, CancellationToken ct = default)
        => await db.ConsumptionDetails.Where(x => x.ConsumptionRm == consumptionRm).ToListAsync(ct);

    public async Task<IEnumerable<ConsumptionDetail>> GetAllAsync(CancellationToken ct = default)
        => await db.ConsumptionDetails.ToListAsync(ct);

    public async Task AddAsync(ConsumptionDetail entity, CancellationToken ct = default)
        => await db.ConsumptionDetails.AddAsync(entity, ct);

    public void Delete(ConsumptionDetail entity) => db.ConsumptionDetails.Remove(entity);
}

public class DispatchDetailRepository(MamAllocationDbContext db) : IDispatchDetailRepository
{
    public async Task<IEnumerable<DispatchDetail>> GetByFgAsync(int dispatchFg, CancellationToken ct = default)
        => await db.DispatchDetails.Where(x => x.DispatchFg == dispatchFg).ToListAsync(ct);

    public async Task<IEnumerable<DispatchDetail>> GetAllAsync(CancellationToken ct = default)
        => await db.DispatchDetails.ToListAsync(ct);

    public async Task AddAsync(DispatchDetail entity, CancellationToken ct = default)
        => await db.DispatchDetails.AddAsync(entity, ct);

    public void Delete(DispatchDetail entity) => db.DispatchDetails.Remove(entity);
}

public class FgAllocationRepository(MamAllocationDbContext db) : IFgAllocationRepository
{
    public async Task<IEnumerable<FgAllocation>> GetAllAsync(CancellationToken ct = default)
        => await db.FgAllocations.ToListAsync(ct);

    public async Task AddAsync(FgAllocation entity, CancellationToken ct = default)
        => await db.FgAllocations.AddAsync(entity, ct);

    public void Delete(FgAllocation entity) => db.FgAllocations.Remove(entity);
}

public class ProductAllocationRepository(MamAllocationDbContext db) : IProductAllocationRepository
{
    public async Task<IEnumerable<ProductAllocation>> GetAllAsync(CancellationToken ct = default)
        => await db.ProductAllocations.ToListAsync(ct);

    public async Task AddAsync(ProductAllocation entity, CancellationToken ct = default)
        => await db.ProductAllocations.AddAsync(entity, ct);

    public void Delete(ProductAllocation entity) => db.ProductAllocations.Remove(entity);
}
