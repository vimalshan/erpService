using HealthTransaction.Domain.Entities;

namespace HealthTransaction.Domain.Interfaces;

public interface IPfiHistoryRepository
{
    Task<IReadOnlyList<PfiHistory>> GetByHlthNumAsync(decimal hlthNum, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PfiHistory>> GetByEmployeeNumAsync(decimal empNum, CancellationToken cancellationToken = default);
    Task AddAsync(PfiHistory entity, CancellationToken cancellationToken = default);
    void Update(PfiHistory entity);
}
