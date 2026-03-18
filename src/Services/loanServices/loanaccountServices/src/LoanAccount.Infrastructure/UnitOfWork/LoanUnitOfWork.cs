using Ardalis.GuardClauses;
using LoanAccount.Domain.Interfaces;
using LoanAccount.Infrastructure.Persistence;
using LoanAccount.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace LoanAccount.Infrastructure.UnitOfWork;

/// <summary>
/// Unit of Work implementation for loan operations
/// </summary>
public class LoanUnitOfWork : ILoanUnitOfWork, IAsyncDisposable, IDisposable
{
    private readonly LoanAccountDbContext _dbContext;
    private IDbContextTransaction? _currentTransaction;

    private ILoanMainRepository? _loanMainRepository;
    private ILoanInstallmentRepository? _installmentRepository;
    private ILoanEmployeeInterestRateRepository? _interestRateRepository;
    private ILoanLedgerRepository? _ledgerRepository;
    private ILoanSettlementRepository? _settlementRepository;

    public ILoanMainRepository LoanMainRepository =>
        _loanMainRepository ??= new LoanMainRepository(_dbContext);

    public ILoanInstallmentRepository InstallmentRepository =>
        _installmentRepository ??= new LoanInstallmentRepository(_dbContext);

    public ILoanEmployeeInterestRateRepository InterestRateRepository =>
        _interestRateRepository ??= new LoanEmployeeInterestRateRepository(_dbContext);

    public ILoanLedgerRepository LedgerRepository =>
        _ledgerRepository ??= new LoanLedgerRepository(_dbContext);

    public ILoanSettlementRepository SettlementRepository =>
        _settlementRepository ??= new LoanSettlementRepository(_dbContext);

    public LoanUnitOfWork(LoanAccountDbContext dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return _currentTransaction is not null;
    }

    public async Task<bool> CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            if (_currentTransaction is not null)
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            return true;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task<bool> RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            return true;
        }
        finally
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.DisposeAsync();
        }
        await _dbContext.DisposeAsync();
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _dbContext.Dispose();
    }
}
