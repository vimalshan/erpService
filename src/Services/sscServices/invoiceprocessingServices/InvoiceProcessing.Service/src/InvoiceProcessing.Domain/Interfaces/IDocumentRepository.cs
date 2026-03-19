using InvoiceProcessing.Domain.Entities;

namespace InvoiceProcessing.Domain.Interfaces;

public interface IDocumentRepository
{
    Task<DocumentDetail?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<DocumentDetail>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<DocumentDetail>> GetByOrgIdAsync(string orgId, CancellationToken ct = default);
    Task<IReadOnlyList<DocumentDetail>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<DocumentDetail> AddAsync(DocumentDetail document, CancellationToken ct = default);
    Task UpdateAsync(DocumentDetail document, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
    Task<bool> ExistsAsync(long id, CancellationToken ct = default);
    Task<(IReadOnlyList<DocumentDetail> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? orgId = null, string? status = null, CancellationToken ct = default);
}
