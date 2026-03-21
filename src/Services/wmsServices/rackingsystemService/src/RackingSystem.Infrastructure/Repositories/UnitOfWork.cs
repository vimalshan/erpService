using RackingSystem.Domain.Interfaces;
using RackingSystem.Infrastructure.Persistence;

namespace RackingSystem.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IRackRepository Racks { get; }
    public IShelfRepository Shelves { get; }
    public IBinRepository Bins { get; }

    public UnitOfWork(ApplicationDbContext context, IRackRepository racks,
        IShelfRepository shelves, IBinRepository bins)
    {
        _context = context;
        Racks    = racks;
        Shelves  = shelves;
        Bins     = bins;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
