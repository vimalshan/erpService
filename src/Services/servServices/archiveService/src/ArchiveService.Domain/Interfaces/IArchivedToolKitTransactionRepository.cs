using ArchiveService.Domain.Entities;

namespace ArchiveService.Domain.Interfaces;

public interface IArchivedToolKitTransactionRepository
{
    Task<IReadOnlyList<ArchivedToolKitTransaction>> GetByToolkitIdAsync(long toolkitId, CancellationToken ct = default);
    Task<ArchivedToolKitTransaction?> GetByIdAsync(long id, CancellationToken ct = default);
    Task AddAsync(ArchivedToolKitTransaction transaction, CancellationToken ct = default);
}
