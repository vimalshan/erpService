namespace SettlementService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ISettlementRepository Settlements { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
