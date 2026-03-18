using BatchService.Domain.Entities;

namespace BatchService.Domain.Interfaces;

public interface IBatchRepository
{
    Task<BatchMaster?> GetByIdAsync(long batchId, CancellationToken ct = default);
    Task<IEnumerable<BatchMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<BatchMaster>> GetByMonthAsync(int monthNo, CancellationToken ct = default);
    Task AddAsync(BatchMaster batch, CancellationToken ct = default);
    Task UpdateAsync(BatchMaster batch, CancellationToken ct = default);
    Task DeleteAsync(long batchId, CancellationToken ct = default);
    Task<bool> ExistsAsync(long batchId, CancellationToken ct = default);
}
