using Microsoft.EntityFrameworkCore;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Domain.Aggregates;
using AimsTransactionService.Infrastructure.Data;

namespace AimsTransactionService.Infrastructure.Repositories;

public class CompOffRepository(AimsTransactionDbContext context) : ICompOffRepository
{
    public async Task<CompOffAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.CompOffs.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<IEnumerable<CompOffAggregate>> GetByEmployeeAsync(
        long employeeSysId, CancellationToken cancellationToken = default)
        => await context.CompOffs
            .Where(c => c.EmployeeSysId == employeeSysId)
            .OrderByDescending(c => c.RequestedOn)
            .ToListAsync(cancellationToken);

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var max = await context.CompOffs.MaxAsync(c => (long?)c.Id, cancellationToken) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(CompOffAggregate compOff, CancellationToken cancellationToken = default)
        => await context.CompOffs.AddAsync(compOff, cancellationToken);

    public void Update(CompOffAggregate compOff)
        => context.CompOffs.Update(compOff);
}
