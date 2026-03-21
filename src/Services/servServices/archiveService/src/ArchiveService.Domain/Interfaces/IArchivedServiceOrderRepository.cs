using ArchiveService.Domain.Entities;

namespace ArchiveService.Domain.Interfaces;

public interface IArchivedServiceOrderRepository
{
    Task<ArchivedServiceOrder?> GetByIdAsync(string sernoDell, CancellationToken ct = default);
    Task<ArchivedServiceOrder?> GetBySapIdAsync(string sapId, CancellationToken ct = default);
    Task<IReadOnlyList<ArchivedServiceOrder>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<IReadOnlyList<ArchivedServiceOrder>> SearchAsync(string? branch, string? engineerId, string? callStatus, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<int> GetCountAsync(CancellationToken ct = default);
    Task AddAsync(ArchivedServiceOrder order, CancellationToken ct = default);
    Task UpdateAsync(ArchivedServiceOrder order, CancellationToken ct = default);
    Task DeleteAsync(string sernoDell, CancellationToken ct = default);
}
