namespace travelTransactionService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVendorMasterRepository Vendors { get; }
    ITaxMasterRepository TaxMasters { get; }
    IJaiInterfaceLineRepository JaiInterfaceLines { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
