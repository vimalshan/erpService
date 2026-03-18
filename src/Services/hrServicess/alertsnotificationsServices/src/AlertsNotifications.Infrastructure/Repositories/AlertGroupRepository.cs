using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Interfaces;
using AlertsNotifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlertsNotifications.Infrastructure.Repositories;

public class AlertGroupRepository : IAlertGroupRepository
{
    private readonly AlertsNotificationsDbContext _context;

    public AlertGroupRepository(AlertsNotificationsDbContext context)
    {
        _context = context;
    }

    public async Task<AlertGroup?> GetByIdAsync(decimal alertGroupId, CancellationToken cancellationToken = default)
    {
        return await _context.AlertGroups.FindAsync(new object[] { alertGroupId }, cancellationToken);
    }

    public async Task<IEnumerable<AlertGroup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AlertGroups.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<AlertGroup> AddAsync(AlertGroup alertGroup, CancellationToken cancellationToken = default)
    {
        _context.AlertGroups.Add(alertGroup);
        await _context.SaveChangesAsync(cancellationToken);
        return alertGroup;
    }

    public async Task UpdateAsync(AlertGroup alertGroup, CancellationToken cancellationToken = default)
    {
        _context.Entry(alertGroup).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(decimal alertGroupId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.AlertGroups.FindAsync(new object[] { alertGroupId }, cancellationToken);
        if (entity is not null)
        {
            _context.AlertGroups.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
