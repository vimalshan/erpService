using LoanTransaction.Domain.Interfaces;
using LoanTransaction.Infrastructure.Data;
using LoanTransaction.Infrastructure.Repositories;

namespace LoanTransaction.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly LoanTransactionDbContext _ctx;
    private ILoanRepository? _loans;
    private ILoanInstallmentRepository? _installments;
    private ILoanSettlementRepository? _settlements;
    private ILoanLedgerRepository? _ledgerEntries;

    public UnitOfWork(LoanTransactionDbContext ctx) => _ctx = ctx;

    public ILoanRepository Loans => _loans ??= new LoanRepository(_ctx);
    public ILoanInstallmentRepository Installments => _installments ??= new LoanInstallmentRepository(_ctx);
    public ILoanSettlementRepository Settlements => _settlements ??= new LoanSettlementRepository(_ctx);
    public ILoanLedgerRepository LedgerEntries => _ledgerEntries ??= new LoanLedgerRepository(_ctx);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => await _ctx.Database.BeginTransactionAsync(ct);

    public async Task CommitAsync(CancellationToken ct = default)
    {
        try
        {
            await _ctx.SaveChangesAsync(ct);
            if (_ctx.Database.CurrentTransaction is not null)
                await _ctx.Database.CommitTransactionAsync(ct);
        }
        catch
        {
            await RollbackAsync(ct);
            throw;
        }
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_ctx.Database.CurrentTransaction is not null)
            await _ctx.Database.RollbackTransactionAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _ctx.SaveChangesAsync(ct);

    public async ValueTask DisposeAsync() => await _ctx.DisposeAsync();
}
