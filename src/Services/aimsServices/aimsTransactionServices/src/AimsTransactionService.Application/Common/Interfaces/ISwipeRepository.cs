using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.Common.Interfaces;

public interface ISwipeRepository
{
    Task<SwipeAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SwipeAggregate>> GetByEmployeeAsync(long employeeSysId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(SwipeAggregate swipe, CancellationToken cancellationToken = default);
    void Update(SwipeAggregate swipe);
}
