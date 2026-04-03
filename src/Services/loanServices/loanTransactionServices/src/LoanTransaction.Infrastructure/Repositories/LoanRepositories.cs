using Microsoft.EntityFrameworkCore;
using LoanTransaction.Domain.Aggregates;
using LoanTransaction.Domain.Entities;
using LoanTransaction.Domain.Interfaces;
using LoanTransaction.Infrastructure.Data;

namespace LoanTransaction.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly LoanTransactionDbContext _ctx;

    public LoanRepository(LoanTransactionDbContext ctx) => _ctx = ctx;

    public async Task<LoanAggregate?> GetByIdAsync(long loanNo, CancellationToken ct = default)
        => await _ctx.Loans.AsNoTracking().FirstOrDefaultAsync(x => x.Id == loanNo, ct);

    public async Task<LoanAggregate?> GetByIdWithInstallmentsAsync(long loanNo, CancellationToken ct = default)
    {
        // Load aggregate + installments separately (no navigation in domain)
        var loan = await _ctx.Loans.FirstOrDefaultAsync(x => x.Id == loanNo, ct);
        if (loan is null) return null;
        var installments = await _ctx.LoanInstallments.Where(x => x.LoanNo == loanNo).ToListAsync(ct);
        // Inject via reflection or keep via separate repo; here we return the loan,
        // caller uses ILoanInstallmentRepository for installments
        return loan;
    }

    public async Task<IEnumerable<LoanAggregate>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default)
        => await _ctx.Loans.AsNoTracking().Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.EffectiveDate).ToListAsync(ct);

    public async Task<IEnumerable<LoanAggregate>> GetActiveByEmployeeIdAsync(long employeeId, CancellationToken ct = default)
        => await _ctx.Loans.AsNoTracking()
            .Where(x => x.EmployeeId == employeeId && x.ClosureDate == null)
            .OrderByDescending(x => x.EffectiveDate).ToListAsync(ct);

    public async Task<IEnumerable<LoanAggregate>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        => await _ctx.Loans.AsNoTracking()
            .OrderByDescending(x => x.EffectiveDate)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> GetTotalCountAsync(CancellationToken ct = default)
        => await _ctx.Loans.CountAsync(ct);

    public async Task AddAsync(LoanAggregate loan, CancellationToken ct = default)
        => await _ctx.Loans.AddAsync(loan, ct);

    public Task UpdateAsync(LoanAggregate loan, CancellationToken ct = default)
    {
        _ctx.Loans.Update(loan);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _ctx.SaveChangesAsync(ct);
}

public class LoanInstallmentRepository : ILoanInstallmentRepository
{
    private readonly LoanTransactionDbContext _ctx;

    public LoanInstallmentRepository(LoanTransactionDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<LoanInstallment>> GetByLoanNoAsync(long loanNo, CancellationToken ct = default)
        => await _ctx.LoanInstallments.AsNoTracking()
            .Where(x => x.LoanNo == loanNo).OrderBy(x => x.InstallmentNo).ToListAsync(ct);

    public async Task<LoanInstallment?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _ctx.LoanInstallments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IEnumerable<LoanInstallment>> GetPendingByLoanNoAsync(long loanNo, CancellationToken ct = default)
        => await _ctx.LoanInstallments.AsNoTracking()
            .Where(x => x.LoanNo == loanNo && x.PrincipalRecovered == 0 && x.InterestRecovered == 0)
            .OrderBy(x => x.InstallmentNo).ToListAsync(ct);

    public async Task AddRangeAsync(IEnumerable<LoanInstallment> installments, CancellationToken ct = default)
        => await _ctx.LoanInstallments.AddRangeAsync(installments, ct);

    public Task UpdateAsync(LoanInstallment installment, CancellationToken ct = default)
    {
        _ctx.LoanInstallments.Update(installment);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _ctx.SaveChangesAsync(ct);
}

public class LoanSettlementRepository : ILoanSettlementRepository
{
    private readonly LoanTransactionDbContext _ctx;

    public LoanSettlementRepository(LoanTransactionDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<LoanSettlement>> GetByLoanNoAsync(long loanNo, CancellationToken ct = default)
        => await _ctx.LoanSettlements.AsNoTracking()
            .Where(x => x.LoanNo == loanNo).OrderByDescending(x => x.RecoveryDate).ToListAsync(ct);

    public async Task AddAsync(LoanSettlement settlement, CancellationToken ct = default)
        => await _ctx.LoanSettlements.AddAsync(settlement, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _ctx.SaveChangesAsync(ct);
}

public class LoanLedgerRepository : ILoanLedgerRepository
{
    private readonly LoanTransactionDbContext _ctx;

    public LoanLedgerRepository(LoanTransactionDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<LoanLedger>> GetByLoanNoAsync(long loanNo, CancellationToken ct = default)
        => await _ctx.LoanLedgers.AsNoTracking()
            .Where(x => x.LoanNo == loanNo).OrderBy(x => x.TransactionDate).ToListAsync(ct);

    public async Task<IEnumerable<LoanLedger>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default)
        => await _ctx.LoanLedgers.AsNoTracking()
            .Where(x => x.EmployeeId == employeeId).OrderByDescending(x => x.TransactionDate).ToListAsync(ct);

    public async Task AddAsync(LoanLedger ledger, CancellationToken ct = default)
        => await _ctx.LoanLedgers.AddAsync(ledger, ct);

    public async Task AddRangeAsync(IEnumerable<LoanLedger> entries, CancellationToken ct = default)
        => await _ctx.LoanLedgers.AddRangeAsync(entries, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _ctx.SaveChangesAsync(ct);
}
