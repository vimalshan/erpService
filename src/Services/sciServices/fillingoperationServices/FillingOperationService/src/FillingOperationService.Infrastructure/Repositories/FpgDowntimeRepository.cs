using FillingOperationService.Domain.Entities;
using FillingOperationService.Domain.Interfaces;
using FillingOperationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FillingOperationService.Infrastructure.Repositories;

public class FpgDowntimeRepository(FillingOperationsDbContext context) : IFpgDowntimeRepository
{
    public async Task<FpgDowntime?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.FpgDowntimes.FirstOrDefaultAsync(d => d.FpgId == id, cancellationToken);

    public async Task<IEnumerable<FpgDowntime>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
        => await context.FpgDowntimes
            .AsNoTracking()
            .Where(d => d.FillingPointGroupId == groupId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(FpgDowntime downtime, CancellationToken cancellationToken = default)
        => await context.FpgDowntimes.AddAsync(downtime, cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
