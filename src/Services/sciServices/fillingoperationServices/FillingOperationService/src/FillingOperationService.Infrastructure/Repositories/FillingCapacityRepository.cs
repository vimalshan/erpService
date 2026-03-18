using FillingOperationService.Domain.Entities;
using FillingOperationService.Domain.Interfaces;
using FillingOperationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FillingOperationService.Infrastructure.Repositories;

public class FillingCapacityRepository(FillingOperationsDbContext context) : IFillingCapacityRepository
{
    public async Task<FillingCapacity?> GetByGroupAndProductAsync(int groupId, int productId, CancellationToken cancellationToken = default)
        => await context.FillingCapacities
            .FirstOrDefaultAsync(c => c.FillingPointGroupId == groupId && c.MainProductId == productId, cancellationToken);

    public async Task<IEnumerable<FillingCapacity>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
        => await context.FillingCapacities
            .AsNoTracking()
            .Where(c => c.FillingPointGroupId == groupId)
            .OrderBy(c => c.UsagePriority)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(FillingCapacity capacity, CancellationToken cancellationToken = default)
        => await context.FillingCapacities.AddAsync(capacity, cancellationToken);

    public void Update(FillingCapacity capacity)
        => context.FillingCapacities.Update(capacity);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
