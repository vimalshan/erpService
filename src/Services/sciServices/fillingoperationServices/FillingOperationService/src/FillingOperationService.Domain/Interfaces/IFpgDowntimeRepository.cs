using FillingOperationService.Domain.Entities;

namespace FillingOperationService.Domain.Interfaces;

public interface IFpgDowntimeRepository
{
    Task<FpgDowntime?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FpgDowntime>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);
    Task AddAsync(FpgDowntime downtime, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
