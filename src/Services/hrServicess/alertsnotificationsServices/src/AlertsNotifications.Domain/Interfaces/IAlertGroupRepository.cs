using AlertsNotifications.Domain.Entities;

namespace AlertsNotifications.Domain.Interfaces;

public interface IAlertGroupRepository
{
    Task<AlertGroup?> GetByIdAsync(decimal alertGroupId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AlertGroup>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AlertGroup> AddAsync(AlertGroup alertGroup, CancellationToken cancellationToken = default);
    Task UpdateAsync(AlertGroup alertGroup, CancellationToken cancellationToken = default);
    Task DeleteAsync(decimal alertGroupId, CancellationToken cancellationToken = default);
}
