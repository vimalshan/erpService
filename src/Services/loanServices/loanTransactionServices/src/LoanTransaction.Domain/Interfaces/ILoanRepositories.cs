using LoanTransaction.Domain.Aggregates;
using LoanTransaction.Domain.Entities;

namespace LoanTransaction.Domain.Interfaces;

public interface ILoanRepository
{
    Task<LoanAggregate?> GetByIdAsync(long loanNo, CancellationToken ct = default);
    Task<LoanAggregate?> GetByIdWithInstallmentsAsync(long loanNo, CancellationToken ct = default);
    Task<IEnumerable<LoanAggregate>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default);
    Task<IEnumerable<LoanAggregate>> GetActiveByEmployeeIdAsync(long employeeId, CancellationToken ct = default);
    Task<IEnumerable<LoanAggregate>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    Task<int> GetTotalCountAsync(CancellationToken ct = default);
    Task AddAsync(LoanAggregate loan, CancellationToken ct = default);
    Task UpdateAsync(LoanAggregate loan, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface ILoanInstallmentRepository
{
    Task<IEnumerable<LoanInstallment>> GetByLoanNoAsync(long loanNo, CancellationToken ct = default);
    Task<LoanInstallment?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<LoanInstallment>> GetPendingByLoanNoAsync(long loanNo, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<LoanInstallment> installments, CancellationToken ct = default);
    Task UpdateAsync(LoanInstallment installment, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface ILoanSettlementRepository
{
    Task<IEnumerable<LoanSettlement>> GetByLoanNoAsync(long loanNo, CancellationToken ct = default);
    Task AddAsync(LoanSettlement settlement, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface ILoanLedgerRepository
{
    Task<IEnumerable<LoanLedger>> GetByLoanNoAsync(long loanNo, CancellationToken ct = default);
    Task<IEnumerable<LoanLedger>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default);
    Task AddAsync(LoanLedger ledger, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<LoanLedger> entries, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
