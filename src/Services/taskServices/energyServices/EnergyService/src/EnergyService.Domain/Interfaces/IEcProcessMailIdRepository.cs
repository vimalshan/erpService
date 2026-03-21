using EnergyService.Domain.Entities;

namespace EnergyService.Domain.Interfaces;

public interface IEcProcessMailIdRepository
{
    Task<IReadOnlyList<EcProcessMailId>> GetByProcessIdAsync(int processId, CancellationToken ct = default);
    Task AddAsync(EcProcessMailId entity, CancellationToken ct = default);
    void Update(EcProcessMailId entity);
}
