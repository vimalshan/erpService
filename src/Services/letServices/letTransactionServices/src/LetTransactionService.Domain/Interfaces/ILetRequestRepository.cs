using LetTransactionService.Domain.Entities;

namespace LetTransactionService.Domain.Interfaces;

public interface ILetRequestRepository
{
    Task<LetMain?> GetByIdAsync(long requestNumber, CancellationToken ct = default);
    Task<IEnumerable<LetMain>> GetByEmployeeAsync(string employeeUserId, CancellationToken ct = default);
    Task<IEnumerable<LetMain>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(LetMain letMain, CancellationToken ct = default);
    Task UpdateAsync(LetMain letMain, CancellationToken ct = default);
    Task<bool> ExistsAsync(long requestNumber, CancellationToken ct = default);
}
