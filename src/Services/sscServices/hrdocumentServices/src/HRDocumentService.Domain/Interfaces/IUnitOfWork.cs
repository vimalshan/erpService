namespace HRDocumentService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IHRDocumentRepository HRDocuments { get; }
    IHRDocumentFileRepository HRDocumentFiles { get; }
    IHRDocumentReceiptRepository HRDocumentReceipts { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
