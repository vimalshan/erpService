using HealthTransaction.Domain.Interfaces;
using HealthTransaction.Infrastructure.Persistence;

namespace HealthTransaction.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly HealthTransactionDbContext _context;

    public UnitOfWork(HealthTransactionDbContext context)
    {
        _context = context;
        PreEmploymentCheckups = new PreEmploymentCheckupRepository(context);
        CheckupCards = new CheckupCardRepository(context);
        DynamicHealthDetails = new DynamicHealthDetailRepository(context);
        PfiHistories = new PfiHistoryRepository(context);
    }

    public IPreEmploymentCheckupRepository PreEmploymentCheckups { get; }
    public ICheckupCardRepository CheckupCards { get; }
    public IDynamicHealthDetailRepository DynamicHealthDetails { get; }
    public IPfiHistoryRepository PfiHistories { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
