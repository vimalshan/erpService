using Microsoft.EntityFrameworkCore;
using TeamServices.Domain.Entities;
using TeamServices.Domain.Interfaces;
using TeamServices.Infrastructure.Data;

namespace TeamServices.Infrastructure.Repositories;

public class TeamEmployeeMapRepository : ITeamEmployeeMapRepository
{
    private readonly TeamDbContext _context;

    public TeamEmployeeMapRepository(TeamDbContext context)
    {
        _context = context;
    }

    public async Task<TeamEmployeeMap?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.TeamEmployeeMaps.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<TeamEmployeeMap>> GetByTeamIdAsync(long teamId, CancellationToken cancellationToken = default)
    {
        return await _context.TeamEmployeeMaps
            .Where(e => e.TeamId == teamId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TeamEmployeeMap>> GetActiveByTeamIdAsync(long teamId, DateTime asOfDate, CancellationToken cancellationToken = default)
    {
        return await _context.TeamEmployeeMaps
            .Where(e => e.TeamId == teamId && e.EffectiveDate <= asOfDate && (e.CloseDate == null || e.CloseDate >= asOfDate))
            .ToListAsync(cancellationToken);
    }

    public async Task<TeamEmployeeMap> AddAsync(TeamEmployeeMap employeeMap, CancellationToken cancellationToken = default)
    {
        await _context.TeamEmployeeMaps.AddAsync(employeeMap, cancellationToken);
        return employeeMap;
    }

    public Task UpdateAsync(TeamEmployeeMap employeeMap, CancellationToken cancellationToken = default)
    {
        _context.TeamEmployeeMaps.Update(employeeMap);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.TeamEmployeeMaps.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
            _context.TeamEmployeeMaps.Remove(entity);
    }
}
