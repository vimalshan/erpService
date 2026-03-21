using EnergyService.Domain.Entities;

namespace EnergyService.Domain.Interfaces;

public interface IEcReadingRepository
{
    Task<EcReading?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<EcReading>> GetByProcessIdAsync(int processId, CancellationToken ct = default);
    Task<long?> GetLastReadingValueAsync(string unitCode, int processId, CancellationToken ct = default);
    Task AddAsync(EcReading entity, CancellationToken ct = default);
}
