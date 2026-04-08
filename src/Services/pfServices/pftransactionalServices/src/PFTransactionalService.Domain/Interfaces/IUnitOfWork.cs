namespace PFTransactionalService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPFAccumulationRepository Accumulations { get; }
    IPFSettlementRepository Settlements { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
