using RequestServices.Domain.Aggregates;
using RequestServices.Domain.Entities;

namespace RequestServices.Domain.Interfaces;

public interface IRequestRepository
{
    Task<RequestMain?> GetByIdAsync(long requestId, CancellationToken ct = default);
    Task<IEnumerable<RequestMain>> GetPendingBySuperviorAsync(string supervisorUser, CancellationToken ct = default);
    Task AddAsync(RequestAggregate aggregate, CancellationToken ct = default);
    Task UpdateAsync(RequestAggregate aggregate, CancellationToken ct = default);
    Task<bool> ExistsAsync(long requestId, CancellationToken ct = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
