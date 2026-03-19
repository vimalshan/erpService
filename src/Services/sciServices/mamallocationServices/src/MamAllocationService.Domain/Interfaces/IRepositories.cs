using MamAllocationService.Domain.Entities;

namespace MamAllocationService.Domain.Interfaces;

public interface IAllocationDetailRepository
{
    Task<AllocationDetail?> GetByIdAsync(DateTime allocationDate, int rawMaterialCode, CancellationToken ct = default);
    Task<IEnumerable<AllocationDetail>> GetByDateAsync(DateTime allocationDate, CancellationToken ct = default);
    Task<IEnumerable<AllocationDetail>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(AllocationDetail entity, CancellationToken ct = default);
    void Update(AllocationDetail entity);
    void Delete(AllocationDetail entity);
}

public interface IAllocationProdDetailRepository
{
    Task<IEnumerable<AllocationProdDetail>> GetByDateAsync(DateTime allocationDate, CancellationToken ct = default);
    Task<IEnumerable<AllocationProdDetail>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(AllocationProdDetail entity, CancellationToken ct = default);
    void Delete(AllocationProdDetail entity);
}

public interface IAllocationFgRepository
{
    Task<IEnumerable<AllocationFg>> GetByDateAsync(DateTime allocationDate, CancellationToken ct = default);
    Task<IEnumerable<AllocationFg>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(AllocationFg entity, CancellationToken ct = default);
    void Delete(AllocationFg entity);
}

public interface IArrivalDetailRepository
{
    Task<IEnumerable<ArrivalDetail>> GetByItemAsync(int arrivalItem, CancellationToken ct = default);
    Task<IEnumerable<ArrivalDetail>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ArrivalDetail entity, CancellationToken ct = default);
    void Delete(ArrivalDetail entity);
}

public interface IConsumptionDetailRepository
{
    Task<IEnumerable<ConsumptionDetail>> GetByRmAsync(int consumptionRm, CancellationToken ct = default);
    Task<IEnumerable<ConsumptionDetail>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ConsumptionDetail entity, CancellationToken ct = default);
    void Delete(ConsumptionDetail entity);
}

public interface IDispatchDetailRepository
{
    Task<IEnumerable<DispatchDetail>> GetByFgAsync(int dispatchFg, CancellationToken ct = default);
    Task<IEnumerable<DispatchDetail>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(DispatchDetail entity, CancellationToken ct = default);
    void Delete(DispatchDetail entity);
}

public interface IFgAllocationRepository
{
    Task<IEnumerable<FgAllocation>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(FgAllocation entity, CancellationToken ct = default);
    void Delete(FgAllocation entity);
}

public interface IProductAllocationRepository
{
    Task<IEnumerable<ProductAllocation>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ProductAllocation entity, CancellationToken ct = default);
    void Delete(ProductAllocation entity);
}
