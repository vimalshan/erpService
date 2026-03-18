using Microsoft.EntityFrameworkCore.Storage;
using CashManagement.Domain.Interfaces;

namespace CashManagement.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CashManagementDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(CashManagementDbContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _transaction = await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction is not null) await _transaction.CommitAsync(ct);
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction is not null) await _transaction.RollbackAsync(ct);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
