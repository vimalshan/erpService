using TeamServices.Domain.Entities;

namespace TeamServices.Domain.Interfaces;

public interface ITeamEmployeeMapRepository
{
    Task<TeamEmployeeMap?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TeamEmployeeMap>> GetByTeamIdAsync(long teamId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TeamEmployeeMap>> GetActiveByTeamIdAsync(long teamId, DateTime asOfDate, CancellationToken cancellationToken = default);
    Task<TeamEmployeeMap> AddAsync(TeamEmployeeMap employeeMap, CancellationToken cancellationToken = default);
    Task UpdateAsync(TeamEmployeeMap employeeMap, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
