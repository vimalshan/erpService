using ArchiveService.Domain.Entities;

namespace ArchiveService.Domain.Interfaces;

public interface IArchivedServiceOrderDetailRepository
{
    Task<IReadOnlyList<ArchivedServiceOrderDetail>> GetByServiceOrderAsync(string sernoDell, CancellationToken ct = default);
    Task<ArchivedServiceOrderDetail?> GetByIdAsync(long id, CancellationToken ct = default);
    Task AddAsync(ArchivedServiceOrderDetail detail, CancellationToken ct = default);
    Task UpdateAsync(ArchivedServiceOrderDetail detail, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}
