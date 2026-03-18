using LoanApplication.Domain.Interfaces;
using LoanApplication.Infrastructure.Data;
using LoanApplication.Infrastructure.Repositories;

namespace LoanApplication.Infrastructure.UnitOfWork;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly LoanApplicationDbContext _context;
    private ILoanApplicationRepository? _loanApplicationRepository;

    public UnitOfWork(LoanApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ILoanApplicationRepository LoanApplications =>
        _loanApplicationRepository ??= new LoanApplicationRepository(_context);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CommitTransactionAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.RollbackTransactionAsync(cancellationToken);
            }
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
