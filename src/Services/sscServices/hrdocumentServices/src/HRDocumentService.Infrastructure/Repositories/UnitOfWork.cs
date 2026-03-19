using HRDocumentService.Domain.Interfaces;
using HRDocumentService.Infrastructure.Persistence;

namespace HRDocumentService.Infrastructure.Repositories;

public sealed class UnitOfWork(
    HRDocumentDbContext context,
    IHRDocumentRepository hrDocumentRepository,
    IHRDocumentFileRepository hrDocumentFileRepository,
    IHRDocumentReceiptRepository hrDocumentReceiptRepository)
    : IUnitOfWork
{
    public IHRDocumentRepository HRDocuments => hrDocumentRepository;
    public IHRDocumentFileRepository HRDocumentFiles => hrDocumentFileRepository;
    public IHRDocumentReceiptRepository HRDocumentReceipts => hrDocumentReceiptRepository;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
