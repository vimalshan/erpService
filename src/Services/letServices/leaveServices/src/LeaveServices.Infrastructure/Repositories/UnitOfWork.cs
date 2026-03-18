using LeaveServices.Domain.Repositories;
using LeaveServices.Infrastructure.Persistence;

namespace LeaveServices.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly LeaveDbContext _context;
    public UnitOfWork(LeaveDbContext context) => _context = context;

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
