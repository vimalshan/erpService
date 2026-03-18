using Microsoft.EntityFrameworkCore;
using TeamServices.Domain.Entities;
using TeamServices.Domain.Interfaces;
using TeamServices.Infrastructure.Data;

namespace TeamServices.Infrastructure.Repositories;

public class TeamUnitMapRepository : ITeamUnitMapRepository
{
    private readonly TeamDbContext _context;

    public TeamUnitMapRepository(TeamDbContext context)
    {
        _context = context;
    }

    public async Task<TeamUnitMap?> GetByIdAsync(long mapId, CancellationToken cancellationToken = default)
    {
        return await _context.TeamUnitMaps.FindAsync(new object[] { mapId }, cancellationToken);
    }

    public async Task<IReadOnlyList<TeamUnitMap>> GetByTeamIdAsync(long teamId, CancellationToken cancellationToken = default)
    {
        return await _context.TeamUnitMaps
            .Where(u => u.TeamId == teamId)
            .ToListAsync(cancellationToken);
    }

    public async Task<TeamUnitMap> AddAsync(TeamUnitMap unitMap, CancellationToken cancellationToken = default)
    {
        await _context.TeamUnitMaps.AddAsync(unitMap, cancellationToken);
        return unitMap;
    }

    public Task UpdateAsync(TeamUnitMap unitMap, CancellationToken cancellationToken = default)
    {
        _context.TeamUnitMaps.Update(unitMap);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long mapId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.TeamUnitMaps.FindAsync(new object[] { mapId }, cancellationToken);
        if (entity != null)
            _context.TeamUnitMaps.Remove(entity);
    }
}
