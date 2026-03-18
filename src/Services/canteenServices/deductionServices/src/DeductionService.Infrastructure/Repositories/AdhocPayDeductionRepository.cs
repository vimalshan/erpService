using DeductionService.Domain.Entities;
using DeductionService.Domain.Interfaces;
using DeductionService.Domain.ValueObjects;
using DeductionService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeductionService.Infrastructure.Repositories;

public class AdhocPayDeductionRepository(DeductionDbContext context)
    : IAdhocPayDeductionRepository
{
    public async Task<AdhocPayDeduction?> GetByIdAsync(long systemId, CancellationToken ct = default)
        => await context.AdhocPayDeductions
            .FirstOrDefaultAsync(x => x.SystemId == systemId, ct);

    public async Task<IEnumerable<AdhocPayDeduction>> GetByEmployeeAsync(long employeeNumber, CancellationToken ct = default)
        => await context.AdhocPayDeductions
            .Where(x => x.EmployeeNumber == employeeNumber)
            .ToListAsync(ct);

    public async Task<IEnumerable<AdhocPayDeduction>> GetByMonthYearAsync(MonthYear period, CancellationToken ct = default)
    {
        var prefix = period.ToString();
        return await context.AdhocPayDeductions
            .Where(x => x.TransactionDate != null &&
                        x.TransactionDate.Value.Year == period.Year &&
                        x.TransactionDate.Value.Month == period.Month)
            .ToListAsync(ct);
    }

    public async Task AddAsync(AdhocPayDeduction deduction, CancellationToken ct = default)
        => await context.AdhocPayDeductions.AddAsync(deduction, ct);

    public Task UpdateAsync(AdhocPayDeduction deduction, CancellationToken ct = default)
    {
        context.AdhocPayDeductions.Update(deduction);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<AdhocPayDeductionHistory>> GetHistoryByEmployeeAsync(long employeeNumber, CancellationToken ct = default)
        => await context.AdhocPayDeductionHistories
            .Where(x => x.EmployeeNumber == employeeNumber)
            .ToListAsync(ct);
}
