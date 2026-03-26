using AimsTransactionService.Domain.Entities;

namespace AimsTransactionService.Application.Common.Interfaces;

public interface IAttendanceSummaryRepository
{
    Task<AttendanceSummary?> GetByEmployeeMonthAsync(long employeeSysId, DateTime monthStart, DateTime monthEnd, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(AttendanceSummary summary, CancellationToken cancellationToken = default);
    void Update(AttendanceSummary summary);
}
