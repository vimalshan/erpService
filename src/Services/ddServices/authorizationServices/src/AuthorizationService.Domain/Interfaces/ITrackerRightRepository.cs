namespace AuthorizationService.Domain.Interfaces;

/// <summary>
/// Repository interface for TrackerRight entity
/// </summary>
public interface ITrackerRightRepository
{
    Task<Entities.TrackerRight?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.TrackerRight>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.TrackerRight>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.TrackerRight>> GetByBusinessCodeAsync(string businessCode, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.TrackerRight entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entities.TrackerRight entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
