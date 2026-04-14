namespace WMTransactional.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPurchaseOrderRepository PurchaseOrders { get; }
    IReceivingRepository Receivings { get; }
    ISalesOrderRepository SalesOrders { get; }
    IShipmentRepository Shipments { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
