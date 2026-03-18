using CompetencyService.Domain.Interfaces;

namespace CompetencyService.Infrastructure.Persistence;

public class UnitOfWork(CompetencyDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
