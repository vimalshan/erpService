using TeamServices.Domain.Entities;

namespace TeamServices.Domain.Interfaces;

public interface ITeamRepository
{
    Task<TeamMaster?> GetByIdAsync(long teamId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TeamMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TeamMaster> AddAsync(TeamMaster team, CancellationToken cancellationToken = default);
    Task UpdateAsync(TeamMaster team, CancellationToken cancellationToken = default);
    Task DeleteAsync(long teamId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long teamId, CancellationToken cancellationToken = default);
}
