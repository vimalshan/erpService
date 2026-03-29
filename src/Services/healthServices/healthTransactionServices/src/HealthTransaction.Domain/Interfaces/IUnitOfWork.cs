namespace HealthTransaction.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPreEmploymentCheckupRepository PreEmploymentCheckups { get; }
    ICheckupCardRepository CheckupCards { get; }
    IDynamicHealthDetailRepository DynamicHealthDetails { get; }
    IPfiHistoryRepository PfiHistories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
