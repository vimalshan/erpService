using TdsService.Domain.Entities;
using TdsService.Domain.ValueObjects;

namespace TdsService.Domain.Repositories;

public interface ITdsFileRepository
{
    Task<TdsFile?> GetByIdAsync(long fileId, CancellationToken ct = default);
    Task<IReadOnlyList<TdsFile>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TdsFile>> GetByPanAsync(string panNo, CancellationToken ct = default);
    Task<IReadOnlyList<TdsFile>> GetPendingEmailFilesAsync(CancellationToken ct = default);
    Task AddAsync(TdsFile file, CancellationToken ct = default);
    void Update(TdsFile file);
    void Remove(TdsFile file);
    Task<bool> ExistsAsync(long fileId, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
