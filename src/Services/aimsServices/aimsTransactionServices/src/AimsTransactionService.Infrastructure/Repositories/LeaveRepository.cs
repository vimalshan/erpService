using Microsoft.EntityFrameworkCore;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Domain.Aggregates;
using AimsTransactionService.Infrastructure.Data;

namespace AimsTransactionService.Infrastructure.Repositories;

public class LeaveRepository(AimsTransactionDbContext context) : ILeaveRepository
{
    public async Task<LeaveApplicationAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.LeaveApplications.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    public async Task<IEnumerable<LeaveApplicationAggregate>> GetByEmployeeAsync(
        long employeeSysId, CancellationToken cancellationToken = default)
        => await context.LeaveApplications
            .Where(l => l.EmployeeSysId == employeeSysId)
            .OrderByDescending(l => l.AppliedOn)
            .ToListAsync(cancellationToken);

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var max = await context.LeaveApplications.MaxAsync(l => (long?)l.Id, cancellationToken) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(LeaveApplicationAggregate leave, CancellationToken cancellationToken = default)
        => await context.LeaveApplications.AddAsync(leave, cancellationToken);

    public void Update(LeaveApplicationAggregate leave)
        => context.LeaveApplications.Update(leave);
}
