using CashManagement.Domain.Entities;

namespace CashManagement.Domain.Interfaces.Repositories;

public interface ICashUnitRepository
{
    Task<CashUnit?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<CashUnit>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(CashUnit entity, CancellationToken ct = default);
    Task UpdateAsync(CashUnit entity, CancellationToken ct = default);
    Task<decimal> GetCashInHandAsync(long cashUnitId, DateTime asOfDate, CancellationToken ct = default);
}

public interface ICashTransactionRepository
{
    Task<CashTransaction?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<CashTransaction>> GetByUnitAsync(long cashUnitId, DateTime from, DateTime to, CancellationToken ct = default);
    Task AddAsync(CashTransaction entity, CancellationToken ct = default);
}

public interface IBankAccountRepository
{
    Task<BankAccount?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(BankAccount entity, CancellationToken ct = default);
    Task UpdateAsync(BankAccount entity, CancellationToken ct = default);
    Task<decimal> GetBankBalanceAsync(long bankAccountId, DateTime asOfDate, CancellationToken ct = default);
}

public interface IBankTransactionRepository
{
    Task<BankTransaction?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<BankTransaction>> GetByAccountAsync(long bankAccountId, DateTime from, DateTime to, CancellationToken ct = default);
    Task AddAsync(BankTransaction entity, CancellationToken ct = default);
}

public interface IChequeRegisterRepository
{
    Task<ChequeRegister?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<ChequeRegister>> GetByAccountAsync(long bankAccountId, CancellationToken ct = default);
    Task<bool> ExistsAsync(long bankAccountId, string chequeNumber, CancellationToken ct = default);
    Task AddAsync(ChequeRegister entity, CancellationToken ct = default);
    Task UpdateAsync(ChequeRegister entity, CancellationToken ct = default);
    Task<decimal> GetUnclearedTotalAsync(long bankAccountId, DateTime asOfDate, CancellationToken ct = default);
}

public interface IBankReconciliationRepository
{
    Task<BankReconciliation?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<BankReconciliation>> GetByAccountAsync(long bankAccountId, CancellationToken ct = default);
    Task AddAsync(BankReconciliation entity, CancellationToken ct = default);
}
