namespace MamAllocationService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAllocationDetailRepository AllocationDetails { get; }
    IAllocationProdDetailRepository AllocationProdDetails { get; }
    IAllocationFgRepository AllocationFgs { get; }
    IArrivalDetailRepository ArrivalDetails { get; }
    IConsumptionDetailRepository ConsumptionDetails { get; }
    IDispatchDetailRepository DispatchDetails { get; }
    IFgAllocationRepository FgAllocations { get; }
    IProductAllocationRepository ProductAllocations { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
