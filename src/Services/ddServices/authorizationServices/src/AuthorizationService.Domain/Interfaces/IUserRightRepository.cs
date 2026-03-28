namespace AuthorizationService.Domain.Interfaces;

/// <summary>
/// Repository interface for UserRight entity
/// </summary>
public interface IUserRightRepository
{
    Task<Entities.UserRight?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.UserRight>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.UserRight>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.UserRight>> GetByPinNumberAsync(decimal pinNumber, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.UserRight entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entities.UserRight entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
