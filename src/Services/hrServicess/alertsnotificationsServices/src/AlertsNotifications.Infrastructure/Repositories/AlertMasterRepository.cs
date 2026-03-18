using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Interfaces;
using AlertsNotifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlertsNotifications.Infrastructure.Repositories;

public class AlertMasterRepository : IAlertMasterRepository
{
    private readonly AlertsNotificationsDbContext _context;

    public AlertMasterRepository(AlertsNotificationsDbContext context)
    {
        _context = context;
    }

    public async Task<AlertMaster?> GetByIdAsync(decimal alertId, CancellationToken cancellationToken = default)
    {
        return await _context.AlertMasters.FindAsync(new object[] { alertId }, cancellationToken);
    }

    public async Task<IEnumerable<AlertMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AlertMasters.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AlertMaster>> GetByAppAsync(string alertApps, CancellationToken cancellationToken = default)
    {
        return await _context.AlertMasters
            .AsNoTracking()
            .Where(a => a.AlertApps == alertApps)
            .ToListAsync(cancellationToken);
    }

    public async Task<AlertMaster> AddAsync(AlertMaster alertMaster, CancellationToken cancellationToken = default)
    {
        _context.AlertMasters.Add(alertMaster);
        await _context.SaveChangesAsync(cancellationToken);
        return alertMaster;
    }

    public async Task UpdateAsync(AlertMaster alertMaster, CancellationToken cancellationToken = default)
    {
        _context.Entry(alertMaster).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(decimal alertId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.AlertMasters.FindAsync(new object[] { alertId }, cancellationToken);
        if (entity is not null)
        {
            _context.AlertMasters.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
