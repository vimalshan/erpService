using TeamServices.Domain.Entities;

namespace TeamServices.Domain.Interfaces;

public interface ITeamUnitMapRepository
{
    Task<TeamUnitMap?> GetByIdAsync(long mapId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TeamUnitMap>> GetByTeamIdAsync(long teamId, CancellationToken cancellationToken = default);
    Task<TeamUnitMap> AddAsync(TeamUnitMap unitMap, CancellationToken cancellationToken = default);
    Task UpdateAsync(TeamUnitMap unitMap, CancellationToken cancellationToken = default);
    Task DeleteAsync(long mapId, CancellationToken cancellationToken = default);
}
