using DeductionService.Domain.Entities;
using DeductionService.Domain.Interfaces;
using DeductionService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeductionService.Infrastructure.Repositories;

public class DeductionAccessRepository(DeductionDbContext context)
    : IDeductionAccessRepository
{
    public async Task<DeductionAccess?> GetByAccessNumberAsync(long accessNumber, CancellationToken ct = default)
        => await context.DeductionAccesses
            .FirstOrDefaultAsync(x => x.AccessNumber == accessNumber, ct);

    public async Task<IEnumerable<DeductionAccess>> GetActiveByUnitAsync(long unitCode, CancellationToken ct = default)
        => await context.DeductionAccesses
            .Where(x => x.UnitCode == unitCode && x.ClosedOn == null)
            .ToListAsync(ct);

    public async Task AddAsync(DeductionAccess access, CancellationToken ct = default)
        => await context.DeductionAccesses.AddAsync(access, ct);

    public Task UpdateAsync(DeductionAccess access, CancellationToken ct = default)
    {
        context.DeductionAccesses.Update(access);
        return Task.CompletedTask;
    }
}
