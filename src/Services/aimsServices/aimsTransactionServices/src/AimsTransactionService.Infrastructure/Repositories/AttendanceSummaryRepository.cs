using Microsoft.EntityFrameworkCore;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Domain.Entities;
using AimsTransactionService.Infrastructure.Data;

namespace AimsTransactionService.Infrastructure.Repositories;

public class AttendanceSummaryRepository(AimsTransactionDbContext context) : IAttendanceSummaryRepository
{
    public async Task<AttendanceSummary?> GetByEmployeeMonthAsync(
        long employeeSysId, DateTime monthStart, DateTime monthEnd, CancellationToken cancellationToken = default)
        => await context.AttendanceSummaries
            .FirstOrDefaultAsync(s =>
                s.EmployeeSysId == employeeSysId &&
                s.MonthStart == monthStart &&
                s.MonthEnd == monthEnd, cancellationToken);

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var max = await context.AttendanceSummaries.MaxAsync(s => (long?)s.Id, cancellationToken) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(AttendanceSummary summary, CancellationToken cancellationToken = default)
        => await context.AttendanceSummaries.AddAsync(summary, cancellationToken);

    public void Update(AttendanceSummary summary)
        => context.AttendanceSummaries.Update(summary);
}
