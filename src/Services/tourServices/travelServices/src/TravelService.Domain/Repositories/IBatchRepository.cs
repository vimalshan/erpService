using TravelService.Domain.Entities.Batch;

namespace TravelService.Domain.Repositories;

public interface IBatchRepository
{
    Task<BatchMain?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BatchMain>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<BatchMain> AddAsync(BatchMain batch, CancellationToken cancellationToken = default);
    Task UpdateAsync(BatchMain batch, CancellationToken cancellationToken = default);
}
