using LocationServices.Domain.Repositories;
using LocationServices.Infrastructure.Data;
using LocationServices.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace LocationServices.Infrastructure.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly LocationDbContext _ctx;
    private IDbContextTransaction? _transaction;
    private EfLocationAppMapRepository? _locationAppMaps;

    public UnitOfWork(LocationDbContext ctx) => _ctx = ctx;

    public ILocationAppMapRepository LocationAppMaps
        => _locationAppMaps ??= new EfLocationAppMapRepository(_ctx);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _ctx.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _transaction = await _ctx.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(_transaction, nameof(_transaction));
        await _transaction.CommitAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _ctx.Dispose();
    }
}
