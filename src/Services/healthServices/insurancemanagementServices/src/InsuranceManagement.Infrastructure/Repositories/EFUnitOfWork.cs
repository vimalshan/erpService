using Microsoft.EntityFrameworkCore.Storage;
using InsuranceManagement.Infrastructure.Data;

namespace InsuranceManagement.Infrastructure.Repositories;

/// <summary>
/// Entity Framework Unit of Work implementation
/// </summary>
public class EFUnitOfWork : IInsuranceManagementUnitOfWork
{
    private readonly InsuranceManagementDbContext _context;
    private IDbContextTransaction? _transaction;

    private IInsurancePlanRepository? _planRepository;
    private IInsuranceEnrollmentRepository? _enrollmentRepository;
    private IInsuranceClaimRepository? _claimRepository;

    public EFUnitOfWork(InsuranceManagementDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IInsurancePlanRepository PlanRepository
    {
        get
        {
            return _planRepository ??= new InsurancePlanRepository(_context);
        }
    }

    public IInsuranceEnrollmentRepository EnrollmentRepository
    {
        get
        {
            return _enrollmentRepository ??= new InsuranceEnrollmentRepository(_context);
        }
    }

    public IInsuranceClaimRepository ClaimRepository
    {
        get
        {
            return _claimRepository ??= new InsuranceClaimRepository(_context);
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
        await _context.DisposeAsync();
    }
}
