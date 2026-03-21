namespace SalesOrderService.Domain.Interfaces;

public interface IUnitOfWork
{
    ISalesOrderRepository SalesOrders { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
