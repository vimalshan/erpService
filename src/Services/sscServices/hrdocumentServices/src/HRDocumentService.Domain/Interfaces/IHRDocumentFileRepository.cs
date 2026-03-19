using HRDocumentService.Domain.Entities;

namespace HRDocumentService.Domain.Interfaces;

public interface IHRDocumentFileRepository
{
    Task<HRDocumentFile?> GetByIdAsync(long fileId, CancellationToken ct = default);
    Task<IReadOnlyList<HRDocumentFile>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task AddAsync(HRDocumentFile file, CancellationToken ct = default);
    void Delete(HRDocumentFile file);
}
