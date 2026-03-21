namespace ArchiveService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IArchivedServiceOrderRepository ServiceOrders { get; }
    IArchivedServiceOrderDetailRepository ServiceOrderDetails { get; }
    IArchivedToolKitRepository ToolKits { get; }
    IArchivedToolKitTransactionRepository ToolKitTransactions { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
