using Microsoft.EntityFrameworkCore;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Domain.Aggregates;
using AimsTransactionService.Infrastructure.Data;

namespace AimsTransactionService.Infrastructure.Repositories;

public class AttendanceBatchRepository(AimsTransactionDbContext context) : IAttendanceBatchRepository
{
    public async Task<AttendanceBatchAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.AttendanceBatches.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<AttendanceBatchAggregate?> GetByMonthAsync(
        DateTime monthStart, DateTime monthEnd, CancellationToken cancellationToken = default)
        => await context.AttendanceBatches
            .FirstOrDefaultAsync(b => b.MonthStart == monthStart && b.MonthEnd == monthEnd, cancellationToken);

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var max = await context.AttendanceBatches.MaxAsync(b => (long?)b.Id, cancellationToken) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(AttendanceBatchAggregate batch, CancellationToken cancellationToken = default)
        => await context.AttendanceBatches.AddAsync(batch, cancellationToken);

    public void Update(AttendanceBatchAggregate batch)
        => context.AttendanceBatches.Update(batch);
}
