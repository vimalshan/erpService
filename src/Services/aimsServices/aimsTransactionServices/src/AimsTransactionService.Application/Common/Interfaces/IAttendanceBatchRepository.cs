using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.Common.Interfaces;

public interface IAttendanceBatchRepository
{
    Task<AttendanceBatchAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<AttendanceBatchAggregate?> GetByMonthAsync(DateTime monthStart, DateTime monthEnd, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(AttendanceBatchAggregate batch, CancellationToken cancellationToken = default);
    void Update(AttendanceBatchAggregate batch);
}
