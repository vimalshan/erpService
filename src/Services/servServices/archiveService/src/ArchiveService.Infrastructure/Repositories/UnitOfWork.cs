using ArchiveService.Domain.Interfaces;
using ArchiveService.Infrastructure.Persistence;

namespace ArchiveService.Infrastructure.Repositories;

public class UnitOfWork(
    ArchiveDbContext context,
    IArchivedServiceOrderRepository serviceOrders,
    IArchivedServiceOrderDetailRepository serviceOrderDetails,
    IArchivedToolKitRepository toolKits,
    IArchivedToolKitTransactionRepository toolKitTransactions) : IUnitOfWork
{
    public IArchivedServiceOrderRepository ServiceOrders { get; } = serviceOrders;
    public IArchivedServiceOrderDetailRepository ServiceOrderDetails { get; } = serviceOrderDetails;
    public IArchivedToolKitRepository ToolKits { get; } = toolKits;
    public IArchivedToolKitTransactionRepository ToolKitTransactions { get; } = toolKitTransactions;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
