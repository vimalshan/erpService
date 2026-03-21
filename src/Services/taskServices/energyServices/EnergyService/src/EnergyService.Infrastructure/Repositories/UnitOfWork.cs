using EnergyService.Domain.Interfaces;
using EnergyService.Infrastructure.Persistence;

namespace EnergyService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly EnergyDbContext _context;

    public UnitOfWork(EnergyDbContext context)
    {
        _context = context;
        Processes = new EcProcessRepository(context);
        Readings = new EcReadingRepository(context);
        ProcessAccesses = new EcProcessAccessRepository(context);
        ProcessMailIds = new EcProcessMailIdRepository(context);
    }

    public IEcProcessRepository Processes { get; }
    public IEcReadingRepository Readings { get; }
    public IEcProcessAccessRepository ProcessAccesses { get; }
    public IEcProcessMailIdRepository ProcessMailIds { get; }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public void Dispose() => _context.Dispose();
}
