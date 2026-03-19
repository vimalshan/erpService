using HRDocumentService.Domain.Entities;

namespace HRDocumentService.Domain.Interfaces;

public interface IHRDocumentReceiptRepository
{
    Task<HRDocumentReceipt?> GetByIdAsync(long recId, CancellationToken ct = default);
    Task<IReadOnlyList<HRDocumentReceipt>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task AddAsync(HRDocumentReceipt receipt, CancellationToken ct = default);
}
