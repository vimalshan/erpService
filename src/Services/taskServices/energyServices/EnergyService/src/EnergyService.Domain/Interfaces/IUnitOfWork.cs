namespace EnergyService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEcProcessRepository Processes { get; }
    IEcReadingRepository Readings { get; }
    IEcProcessAccessRepository ProcessAccesses { get; }
    IEcProcessMailIdRepository ProcessMailIds { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
