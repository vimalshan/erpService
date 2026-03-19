using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Domain.Interfaces;

public interface INormsRepository
{
    Task<NormsMain?> GetByIdAsync(long normNo, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NormsMain>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<NormsMain> AddAsync(NormsMain norm, CancellationToken cancellationToken = default);
    Task UpdateAsync(NormsMain norm, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NormsMaster>> GetNormsMastersByNormNoAsync(long normNo, CancellationToken cancellationToken = default);
}
