using AlertsNotifications.Domain.Entities;

namespace AlertsNotifications.Domain.Interfaces;

public interface ICircularSignatoryRepository
{
    Task<CircularSignatory?> GetByIdAsync(long signatoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CircularSignatory>> GetByUnitIdAsync(long unitId, CancellationToken cancellationToken = default);
    Task<CircularSignatory> AddAsync(CircularSignatory signatory, CancellationToken cancellationToken = default);
    Task UpdateAsync(CircularSignatory signatory, CancellationToken cancellationToken = default);
    Task DeleteAsync(long signatoryId, CancellationToken cancellationToken = default);
}
