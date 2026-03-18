using FillingOperationService.Domain.Entities;

namespace FillingOperationService.Domain.Interfaces;

public interface IFillingCapacityRepository
{
    Task<FillingCapacity?> GetByGroupAndProductAsync(int groupId, int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FillingCapacity>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);
    Task AddAsync(FillingCapacity capacity, CancellationToken cancellationToken = default);
    void Update(FillingCapacity capacity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
