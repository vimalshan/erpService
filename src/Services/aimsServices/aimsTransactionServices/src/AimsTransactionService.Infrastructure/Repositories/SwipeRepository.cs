using Microsoft.EntityFrameworkCore;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Domain.Aggregates;
using AimsTransactionService.Infrastructure.Data;

namespace AimsTransactionService.Infrastructure.Repositories;

public class SwipeRepository(AimsTransactionDbContext context) : ISwipeRepository
{
    public async Task<SwipeAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.Swipes.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<IEnumerable<SwipeAggregate>> GetByEmployeeAsync(
        long employeeSysId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
        => await context.Swipes
            .Where(s => s.EmployeeSysId == employeeSysId && s.PunchTime >= fromDate && s.PunchTime <= toDate)
            .OrderBy(s => s.PunchTime)
            .ToListAsync(cancellationToken);

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var max = await context.Swipes.MaxAsync(s => (long?)s.Id, cancellationToken) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(SwipeAggregate swipe, CancellationToken cancellationToken = default)
        => await context.Swipes.AddAsync(swipe, cancellationToken);

    public void Update(SwipeAggregate swipe)
        => context.Swipes.Update(swipe);
}
