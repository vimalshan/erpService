using Microsoft.EntityFrameworkCore;
using CashManagement.Domain.Entities;
using CashManagement.Domain.Interfaces.Repositories;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Infrastructure.Persistence.Repositories;

public class CashUnitRepository : ICashUnitRepository
{
    private readonly CashManagementDbContext _context;
    public CashUnitRepository(CashManagementDbContext context) => _context = context;

    public async Task<CashUnit?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.CashUnits.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<CashUnit>> GetAllAsync(CancellationToken ct = default)
        => await _context.CashUnits.ToListAsync(ct);

    public async Task AddAsync(CashUnit entity, CancellationToken ct = default)
        => await _context.CashUnits.AddAsync(entity, ct);

    public Task UpdateAsync(CashUnit entity, CancellationToken ct = default)
    {
        _context.CashUnits.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<decimal> GetCashInHandAsync(long cashUnitId, DateTime asOfDate, CancellationToken ct = default)
    {
        var sum = await _context.CashTransactions
            .Where(t => t.CashUnitId == cashUnitId
                     && t.TxnDate <= asOfDate
                     && t.Status == TransactionStatus.Posted)
            .SumAsync(t => t.TxnType == CashTransactionType.Receipt ? t.Amount : -t.Amount, ct);
        return sum;
    }
}

public class CashTransactionRepository : ICashTransactionRepository
{
    private readonly CashManagementDbContext _context;
    public CashTransactionRepository(CashManagementDbContext context) => _context = context;

    public async Task<CashTransaction?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.CashTransactions.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<CashTransaction>> GetByUnitAsync(long cashUnitId, DateTime from, DateTime to, CancellationToken ct = default)
        => await _context.CashTransactions
            .Where(t => t.CashUnitId == cashUnitId && t.TxnDate >= from && t.TxnDate <= to)
            .OrderByDescending(t => t.TxnDate)
            .ToListAsync(ct);

    public async Task AddAsync(CashTransaction entity, CancellationToken ct = default)
        => await _context.CashTransactions.AddAsync(entity, ct);
}

public class BankAccountRepository : IBankAccountRepository
{
    private readonly CashManagementDbContext _context;
    public BankAccountRepository(CashManagementDbContext context) => _context = context;

    public async Task<BankAccount?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.BankAccounts.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken ct = default)
        => await _context.BankAccounts.ToListAsync(ct);

    public async Task AddAsync(BankAccount entity, CancellationToken ct = default)
        => await _context.BankAccounts.AddAsync(entity, ct);

    public Task UpdateAsync(BankAccount entity, CancellationToken ct = default)
    {
        _context.BankAccounts.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<decimal> GetBankBalanceAsync(long bankAccountId, DateTime asOfDate, CancellationToken ct = default)
    {
        return await _context.BankTransactions
            .Where(t => t.BankAccountId == bankAccountId
                     && t.TxnDate <= asOfDate
                     && t.Status == TransactionStatus.Posted)
            .SumAsync(t => t.TxnType == BankTransactionType.Deposit ? t.Amount : -t.Amount, ct);
    }
}

public class BankTransactionRepository : IBankTransactionRepository
{
    private readonly CashManagementDbContext _context;
    public BankTransactionRepository(CashManagementDbContext context) => _context = context;

    public async Task<BankTransaction?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.BankTransactions.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<BankTransaction>> GetByAccountAsync(long bankAccountId, DateTime from, DateTime to, CancellationToken ct = default)
        => await _context.BankTransactions
            .Where(t => t.BankAccountId == bankAccountId && t.TxnDate >= from && t.TxnDate <= to)
            .OrderByDescending(t => t.TxnDate)
            .ToListAsync(ct);

    public async Task AddAsync(BankTransaction entity, CancellationToken ct = default)
        => await _context.BankTransactions.AddAsync(entity, ct);
}

public class ChequeRegisterRepository : IChequeRegisterRepository
{
    private readonly CashManagementDbContext _context;
    public ChequeRegisterRepository(CashManagementDbContext context) => _context = context;

    public async Task<ChequeRegister?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.ChequeRegisters.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<ChequeRegister>> GetByAccountAsync(long bankAccountId, CancellationToken ct = default)
        => await _context.ChequeRegisters
            .Where(c => c.BankAccountId == bankAccountId)
            .OrderByDescending(c => c.IssueDate)
            .ToListAsync(ct);

    public async Task<bool> ExistsAsync(long bankAccountId, string chequeNumber, CancellationToken ct = default)
        => await _context.ChequeRegisters.AnyAsync(c =>
            c.BankAccountId == bankAccountId && c.ChequeNumber == chequeNumber
            && c.Status != ChequeStatus.Cancelled && c.Status != ChequeStatus.Cleared, ct);

    public async Task AddAsync(ChequeRegister entity, CancellationToken ct = default)
        => await _context.ChequeRegisters.AddAsync(entity, ct);

    public Task UpdateAsync(ChequeRegister entity, CancellationToken ct = default)
    {
        _context.ChequeRegisters.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<decimal> GetUnclearedTotalAsync(long bankAccountId, DateTime asOfDate, CancellationToken ct = default)
        => await _context.ChequeRegisters
            .Where(c => c.BankAccountId == bankAccountId
                     && c.IssueDate <= DateOnly.FromDateTime(asOfDate)
                     && (c.Status == ChequeStatus.Issued || c.Status == ChequeStatus.Bounced))
            .SumAsync(c => c.ChequeAmount, ct);
}

public class BankReconciliationRepository : IBankReconciliationRepository
{
    private readonly CashManagementDbContext _context;
    public BankReconciliationRepository(CashManagementDbContext context) => _context = context;

    public async Task<BankReconciliation?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.BankReconciliations.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<BankReconciliation>> GetByAccountAsync(long bankAccountId, CancellationToken ct = default)
        => await _context.BankReconciliations
            .Where(r => r.BankAccountId == bankAccountId)
            .OrderByDescending(r => r.ReconciliationDate)
            .ToListAsync(ct);

    public async Task AddAsync(BankReconciliation entity, CancellationToken ct = default)
        => await _context.BankReconciliations.AddAsync(entity, ct);
}
