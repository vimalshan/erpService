using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork(SalesOrderDbContext db, ISalesOrderRepository salesOrders) : IUnitOfWork
{
    public ISalesOrderRepository SalesOrders => salesOrders;

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
