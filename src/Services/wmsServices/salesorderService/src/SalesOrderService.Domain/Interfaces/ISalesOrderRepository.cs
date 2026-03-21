using SalesOrderService.Domain.Entities;

namespace SalesOrderService.Domain.Interfaces;

public interface ISalesOrderRepository
{
    Task<SalesOrder?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<SalesOrder?> GetBySoNumberAsync(string soNumber, CancellationToken ct = default);
    Task<IEnumerable<SalesOrder>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<SalesOrder>> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);
    Task AddAsync(SalesOrder order, CancellationToken ct = default);
    void Update(SalesOrder order);
    void Delete(SalesOrder order);
}
