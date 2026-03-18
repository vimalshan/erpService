using LoanAccount.Domain.Entities;

namespace LoanAccount.Domain.Interfaces;

/// <summary>
/// Repository interface for loan main aggregate
/// </summary>
public interface ILoanMainRepository
{
    Task<LoanMain?> GetByLoanNumberAsync(long loanNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanMain>> GetByEmployeeAsync(long empSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanMain>> GetByUnitAsync(long unitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanMain>> GetActiveLoansAsync(CancellationToken cancellationToken = default);
    Task<LoanMain> AddAsync(LoanMain entity, CancellationToken cancellationToken = default);
    void Update(LoanMain entity);
}

/// <summary>
/// Repository interface for loan installments
/// </summary>
public interface ILoanInstallmentRepository
{
    Task<IEnumerable<LoanInstallment>> GetByLoanNoAsync(long loanNo, CancellationToken cancellationToken = default);
    Task<LoanInstallment?> GetByInstallmentIdAsync(long installmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanInstallment>> GetPendingInstallmentsAsync(long loanNo, CancellationToken cancellationToken = default);
    Task<LoanInstallment> AddAsync(LoanInstallment entity, CancellationToken cancellationToken = default);
    void Update(LoanInstallment entity);
}

/// <summary>
/// Repository interface for employee interest rates
/// </summary>
public interface ILoanEmployeeInterestRateRepository
{
    Task<LoanEmployeeInterestRate?> GetByLoanNoAsync(long loanNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanEmployeeInterestRate>> GetActiveRatesAsync(CancellationToken cancellationToken = default);
    Task<LoanEmployeeInterestRate> AddAsync(LoanEmployeeInterestRate entity, CancellationToken cancellationToken = default);
    void Update(LoanEmployeeInterestRate entity);
}

/// <summary>
/// Repository interface for loan ledger
/// </summary>
public interface ILoanLedgerRepository
{
    Task<IEnumerable<LoanLedger>> GetByLoanNoAsync(long loanNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanLedger>> GetByEmployeeAsync(long empSysId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalDebitsAsync(long loanNo, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalCreditsAsync(long loanNo, CancellationToken cancellationToken = default);
    Task<LoanLedger> AddAsync(LoanLedger entity, CancellationToken cancellationToken = default);
    void Update(LoanLedger entity);
}

/// <summary>
/// Repository interface for loan settlements
/// </summary>
public interface ILoanSettlementRepository
{
    Task<IEnumerable<LoanSettlement>> GetByLoanNoAsync(long loanNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanSettlement>> GetBetweenDatesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<LoanSettlement> AddAsync(LoanSettlement entity, CancellationToken cancellationToken = default);
    void Update(LoanSettlement entity);
}

/// <summary>
/// Unit of Work pattern for loan operations
/// </summary>
public interface ILoanUnitOfWork : IDisposable, IAsyncDisposable
{
    ILoanMainRepository LoanMainRepository { get; }
    ILoanInstallmentRepository InstallmentRepository { get; }
    ILoanEmployeeInterestRateRepository InterestRateRepository { get; }
    ILoanLedgerRepository LedgerRepository { get; }
    ILoanSettlementRepository SettlementRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<bool> CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task<bool> RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
