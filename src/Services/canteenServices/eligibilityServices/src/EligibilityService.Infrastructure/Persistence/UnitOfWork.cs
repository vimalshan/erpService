using EligibilityService.Domain.Interfaces;

namespace EligibilityService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly EligibilityDbContext _context;

    public UnitOfWork(EligibilityDbContext context) => _context = context;

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);

    public void Dispose() => _context.Dispose();
}
