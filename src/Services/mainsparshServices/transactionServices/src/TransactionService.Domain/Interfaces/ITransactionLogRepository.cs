using TransactionService.Domain.Entities;

namespace TransactionService.Domain.Interfaces;

public interface ITransactionLogRepository
{
    Task<TransactionLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TransactionLog>> GetByEntityAsync(string transactionType, long transactionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TransactionLog>> GetByActionAsync(string action, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TransactionLog>> GetAllAsync(CancellationToken cancellationToken = default);
    IQueryable<TransactionLog> GetQueryable();
    Task AddAsync(TransactionLog log, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
