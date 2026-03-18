using AuditService.Domain.Entities;

namespace AuditService.Domain.Interfaces;

public interface IObservationRepository
{
    Task<AuditObservation?> GetByIdAsync(long obvId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditObservation>> GetByAuditIdAsync(long auditId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditObservation>> GetPendingObservationsAsync(CancellationToken cancellationToken = default);
    Task AddAsync(AuditObservation observation, CancellationToken cancellationToken = default);
    Task UpdateAsync(AuditObservation observation, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long obvId, CancellationToken cancellationToken = default);
}
