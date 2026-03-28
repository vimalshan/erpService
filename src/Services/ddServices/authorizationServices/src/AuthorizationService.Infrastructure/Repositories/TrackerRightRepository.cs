using Microsoft.EntityFrameworkCore;
using AuthorizationService.Domain.Entities;
using AuthorizationService.Domain.Interfaces;

namespace AuthorizationService.Infrastructure.Repositories;

public class TrackerRightRepository : ITrackerRightRepository
{
    private readonly Data.AuthorizationDbContext _context;

    public TrackerRightRepository(Data.AuthorizationDbContext context)
    {
        _context = context;
    }

    public async Task<TrackerRight?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.TrackerRights
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<TrackerRight>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TrackerRights
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TrackerRight>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.TrackerRights
            .Where(a => a.UserId == userId && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TrackerRight>> GetByBusinessCodeAsync(string businessCode, CancellationToken cancellationToken = default)
    {
        return await _context.TrackerRights
            .Where(a => a.BusinessCode == businessCode && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TrackerRight entity, CancellationToken cancellationToken = default)
    {
        await _context.TrackerRights.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(TrackerRight entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.TrackerRights.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.TrackerRights.Update(entity);
        }
    }
}
