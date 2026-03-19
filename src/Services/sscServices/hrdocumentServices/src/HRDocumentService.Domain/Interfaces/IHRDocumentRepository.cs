using HRDocumentService.Domain.Entities;

namespace HRDocumentService.Domain.Interfaces;

public interface IHRDocumentRepository
{
    Task<HRDocument?> GetByIdAsync(long docId, CancellationToken ct = default);
    Task<IReadOnlyList<HRDocument>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<HRDocument>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IReadOnlyList<HRDocument>> GetByUserIdAsync(long userId, CancellationToken ct = default);
    Task AddAsync(HRDocument document, CancellationToken ct = default);
    void Update(HRDocument document);
    void Delete(HRDocument document);
}
