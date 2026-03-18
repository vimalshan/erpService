using AlertsNotifications.Domain.Entities;

namespace AlertsNotifications.Domain.Interfaces;

public interface IAlertMasterRepository
{
    Task<AlertMaster?> GetByIdAsync(decimal alertId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AlertMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AlertMaster>> GetByAppAsync(string alertApps, CancellationToken cancellationToken = default);
    Task<AlertMaster> AddAsync(AlertMaster alertMaster, CancellationToken cancellationToken = default);
    Task UpdateAsync(AlertMaster alertMaster, CancellationToken cancellationToken = default);
    Task DeleteAsync(decimal alertId, CancellationToken cancellationToken = default);
}
