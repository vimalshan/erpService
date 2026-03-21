using EnergyService.Domain.Entities;

namespace EnergyService.Domain.Interfaces;

public interface IEcProcessAccessRepository
{
    Task<IReadOnlyList<EcProcessAccess>> GetByProcessIdAsync(int processId, CancellationToken ct = default);
    Task UpsertAsync(EcProcessAccess entity, CancellationToken ct = default);
}
