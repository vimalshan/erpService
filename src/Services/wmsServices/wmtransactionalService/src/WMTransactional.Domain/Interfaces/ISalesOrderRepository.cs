using WMTransactional.Domain.Entities;

namespace WMTransactional.Domain.Interfaces;

public interface ISalesOrderRepository
{
    Task<SalesOrder?> GetByIdAsync(int soId, CancellationToken ct = default);
    Task<SalesOrder?> GetByNumberAsync(string soNumber, CancellationToken ct = default);
    Task<IEnumerable<SalesOrder>> GetByCustomerAsync(int customerId, CancellationToken ct = default);
    Task<IEnumerable<SalesOrder>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IEnumerable<SalesOrder>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(SalesOrder salesOrder, CancellationToken ct = default);
    Task UpdateAsync(SalesOrder salesOrder, CancellationToken ct = default);
}
