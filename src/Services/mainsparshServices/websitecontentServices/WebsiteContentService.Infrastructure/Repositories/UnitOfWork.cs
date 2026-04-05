namespace WebsiteContentService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore.Storage;
using WebsiteContentService.Domain.Repositories;
using WebsiteContentService.Infrastructure.Persistence;

public class UnitOfWork(WebsiteContentDbContext context) : IUnitOfWork
{
    private IWebsitePageRepository? _websitePages;
    private IWebsiteNewsRepository? _websiteNews;
    private IDbContextTransaction? _transaction;

    public IWebsitePageRepository WebsitePages =>
        _websitePages ??= new WebsitePageRepository(context);

    public IWebsiteNewsRepository WebsiteNews =>
        _websiteNews ??= new WebsiteNewsRepository(context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _transaction = await context.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
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

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
            await _transaction.DisposeAsync();

        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
