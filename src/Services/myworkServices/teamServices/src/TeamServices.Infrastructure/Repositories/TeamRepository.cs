using Microsoft.EntityFrameworkCore;
using TeamServices.Domain.Entities;
using TeamServices.Domain.Interfaces;
using TeamServices.Infrastructure.Data;

namespace TeamServices.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly TeamDbContext _context;

    public TeamRepository(TeamDbContext context)
    {
        _context = context;
    }

    public async Task<TeamMaster?> GetByIdAsync(long teamId, CancellationToken cancellationToken = default)
    {
        return await _context.Teams
            .Include(t => t.EmployeeMaps)
            .Include(t => t.UnitMaps)
            .FirstOrDefaultAsync(t => t.Id == teamId, cancellationToken);
    }

    public async Task<IReadOnlyList<TeamMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Teams
            .Include(t => t.EmployeeMaps)
            .Include(t => t.UnitMaps)
            .ToListAsync(cancellationToken);
    }

    public async Task<TeamMaster> AddAsync(TeamMaster team, CancellationToken cancellationToken = default)
    {
        await _context.Teams.AddAsync(team, cancellationToken);
        return team;
    }

    public Task UpdateAsync(TeamMaster team, CancellationToken cancellationToken = default)
    {
        _context.Teams.Update(team);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long teamId, CancellationToken cancellationToken = default)
    {
        var team = await _context.Teams.FindAsync(new object[] { teamId }, cancellationToken);
        if (team != null)
            _context.Teams.Remove(team);
    }

    public async Task<bool> ExistsAsync(long teamId, CancellationToken cancellationToken = default)
    {
        return await _context.Teams.AnyAsync(t => t.Id == teamId, cancellationToken);
    }
}
