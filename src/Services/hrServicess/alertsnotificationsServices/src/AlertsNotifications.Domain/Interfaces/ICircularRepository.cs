using AlertsNotifications.Domain.Entities;

namespace AlertsNotifications.Domain.Interfaces;

public interface ICircularRepository
{
    Task<Circular?> GetByIdAsync(long circularId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Circular>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Circular>> GetByStatusAsync(char status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Circular>> GetByOrgIdAsync(long orgId, CancellationToken cancellationToken = default);
    Task<Circular> AddAsync(Circular circular, CancellationToken cancellationToken = default);
    Task UpdateAsync(Circular circular, CancellationToken cancellationToken = default);
    Task DeleteAsync(long circularId, CancellationToken cancellationToken = default);
}
