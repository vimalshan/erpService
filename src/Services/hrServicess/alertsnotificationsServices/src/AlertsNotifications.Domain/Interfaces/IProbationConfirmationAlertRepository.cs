using AlertsNotifications.Domain.Entities;

namespace AlertsNotifications.Domain.Interfaces;

public interface IProbationConfirmationAlertRepository
{
    Task<ProbationConfirmationAlert?> GetByIdAsync(long probationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProbationConfirmationAlert>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProbationConfirmationAlert>> GetPendingAlertsAsync(CancellationToken cancellationToken = default);
    Task<ProbationConfirmationAlert> AddAsync(ProbationConfirmationAlert alert, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProbationConfirmationAlert alert, CancellationToken cancellationToken = default);
    Task DeleteAsync(long probationId, CancellationToken cancellationToken = default);
}
