using AccountingService.Domain.Entities;

namespace AccountingService.Domain.Interfaces;

public interface ITransactionDetailRepository
{
    Task<TransactionDetail?> GetByIdAsync(string trustCode, int transactionId, CancellationToken ct = default);
    Task<IEnumerable<TransactionDetail>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default);
    Task AddAsync(TransactionDetail entity, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
