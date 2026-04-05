namespace WebsiteContentService.Domain.Repositories;

public interface IUnitOfWork : IAsyncDisposable
{
    IWebsitePageRepository WebsitePages { get; }
    IWebsiteNewsRepository WebsiteNews { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
