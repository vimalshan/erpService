using SwipeTransactionService.Domain.Entities;

namespace SwipeTransactionService.Domain.Interfaces.Repositories;

public interface ISwipeCardUploadRepository
{
    Task<SwipeCardUpload?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct = default);
    Task<IEnumerable<SwipeCardUpload>> GetByEmployeeAsync(string employeeNumber, DateTime from, DateTime to, CancellationToken ct = default);
    Task<IEnumerable<SwipeCardUpload>> GetPendingAsync(CancellationToken ct = default);
    Task AddAsync(SwipeCardUpload entity, CancellationToken ct = default);
    Task UpdateAsync(SwipeCardUpload entity, CancellationToken ct = default);
    Task<long> GetNextSerialNumberAsync(CancellationToken ct = default);
}
