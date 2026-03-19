using MamAllocationService.Domain.Interfaces;
using MamAllocationService.Infrastructure.Persistence;

namespace MamAllocationService.Infrastructure.Repositories;

public class UnitOfWork(
    MamAllocationDbContext db,
    IAllocationDetailRepository allocationDetails,
    IAllocationProdDetailRepository allocationProdDetails,
    IAllocationFgRepository allocationFgs,
    IArrivalDetailRepository arrivalDetails,
    IConsumptionDetailRepository consumptionDetails,
    IDispatchDetailRepository dispatchDetails,
    IFgAllocationRepository fgAllocations,
    IProductAllocationRepository productAllocations) : IUnitOfWork
{
    public IAllocationDetailRepository AllocationDetails => allocationDetails;
    public IAllocationProdDetailRepository AllocationProdDetails => allocationProdDetails;
    public IAllocationFgRepository AllocationFgs => allocationFgs;
    public IArrivalDetailRepository ArrivalDetails => arrivalDetails;
    public IConsumptionDetailRepository ConsumptionDetails => consumptionDetails;
    public IDispatchDetailRepository DispatchDetails => dispatchDetails;
    public IFgAllocationRepository FgAllocations => fgAllocations;
    public IProductAllocationRepository ProductAllocations => productAllocations;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await db.SaveChangesAsync(ct);

    public void Dispose()
    {
        db.Dispose();
        GC.SuppressFinalize(this);
    }
}
