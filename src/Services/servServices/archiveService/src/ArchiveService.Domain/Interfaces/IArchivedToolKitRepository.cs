using ArchiveService.Domain.Entities;

namespace ArchiveService.Domain.Interfaces;

public interface IArchivedToolKitRepository
{
    Task<ArchivedToolKit?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<ArchivedToolKit>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<IReadOnlyList<ArchivedToolKit>> GetByEngineerIdAsync(string engineerId, CancellationToken ct = default);
    Task AddAsync(ArchivedToolKit toolkit, CancellationToken ct = default);
    Task UpdateAsync(ArchivedToolKit toolkit, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}
