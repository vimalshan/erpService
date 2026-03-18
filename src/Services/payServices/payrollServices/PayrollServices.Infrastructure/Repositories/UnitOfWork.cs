using PayrollServices.Domain.Interfaces;
using PayrollServices.Infrastructure.Data;

namespace PayrollServices.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly PayrollDbContext _dbContext;
    private IPayrollRepository? _payrollRepository;

    public UnitOfWork(PayrollDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IPayrollRepository PayrollRepository =>
        _payrollRepository ??= new PayrollRepository(_dbContext);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> BeginTransactionAsync()
    {
        await _dbContext.Database.BeginTransactionAsync();
        return true;
    }

    public async Task<bool> CommitAsync()
    {
        await _dbContext.Database.CommitTransactionAsync();
        return true;
    }

    public async Task<bool> RollbackAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
        return true;
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}
