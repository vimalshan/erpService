using Microsoft.EntityFrameworkCore;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Domain.Entities;
using AimsTransactionService.Infrastructure.Data;

namespace AimsTransactionService.Infrastructure.Repositories;

public class LeaveCreditRepository(AimsTransactionDbContext context) : ILeaveCreditRepository
{
    public async Task<decimal> GetBalanceAsync(
        long employeeSysId, int leaveId, CancellationToken cancellationToken = default)
    {
        return await context.LeaveCredits
            .Where(c => c.EmployeeSysId == employeeSysId && c.LeaveId == leaveId)
            .SumAsync(c => c.CreditDays, cancellationToken);
    }

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var max = await context.LeaveCredits.MaxAsync(c => (long?)c.Id, cancellationToken) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(LeaveCredit credit, CancellationToken cancellationToken = default)
        => await context.LeaveCredits.AddAsync(credit, cancellationToken);
}
