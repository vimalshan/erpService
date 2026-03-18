using AlertsNotifications.Domain.Entities;

namespace AlertsNotifications.Domain.Interfaces;

public interface ICircularTemplateRepository
{
    Task<CircularTemplate?> GetByIdAsync(long templateId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CircularTemplate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CircularTemplate>> GetByTypeIdAsync(long typeId, CancellationToken cancellationToken = default);
    Task<CircularTemplate> AddAsync(CircularTemplate template, CancellationToken cancellationToken = default);
    Task UpdateAsync(CircularTemplate template, CancellationToken cancellationToken = default);
    Task DeleteAsync(long templateId, CancellationToken cancellationToken = default);
}
