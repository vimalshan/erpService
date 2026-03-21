namespace TransactionService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;
    Task<int> CompleteAsync(CancellationToken ct = default);
}
