using BankService.Domain.Entities;

namespace BankService.Domain.Interfaces;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetByIdAsync(long accountId, CancellationToken ct = default);
    Task<BankAccount?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
    Task<IReadOnlyList<BankAccount>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<BankAccount>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default);
    Task AddAsync(BankAccount account, CancellationToken ct = default);
    void Update(BankAccount account);
    void Delete(BankAccount account);
}
