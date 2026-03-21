using ExpenseService.Domain.Entities;

namespace ExpenseService.Domain.Interfaces;

public interface IDaSummaryRepository
{
    Task<DaSummary?> GetByRequestIdAsync(long requestId, CancellationToken ct = default);
    Task<DaSummary> AddAsync(DaSummary summary, CancellationToken ct = default);
    Task UpdateAsync(DaSummary summary, CancellationToken ct = default);
}
