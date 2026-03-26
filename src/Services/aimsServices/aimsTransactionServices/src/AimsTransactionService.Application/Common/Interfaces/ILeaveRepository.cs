using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.Common.Interfaces;

public interface ILeaveRepository
{
    Task<LeaveApplicationAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LeaveApplicationAggregate>> GetByEmployeeAsync(long employeeSysId, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(LeaveApplicationAggregate leave, CancellationToken cancellationToken = default);
    void Update(LeaveApplicationAggregate leave);
}
