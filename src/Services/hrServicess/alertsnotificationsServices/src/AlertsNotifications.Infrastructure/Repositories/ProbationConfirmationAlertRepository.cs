using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Interfaces;
using AlertsNotifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlertsNotifications.Infrastructure.Repositories;

public class ProbationConfirmationAlertRepository : IProbationConfirmationAlertRepository
{
    private readonly AlertsNotificationsDbContext _context;

    public ProbationConfirmationAlertRepository(AlertsNotificationsDbContext context)
    {
        _context = context;
    }

    public async Task<ProbationConfirmationAlert?> GetByIdAsync(long probationId, CancellationToken cancellationToken = default)
    {
        return await _context.ProbationConfirmationAlerts.FindAsync(new object[] { probationId }, cancellationToken);
    }

    public async Task<IEnumerable<ProbationConfirmationAlert>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProbationConfirmationAlerts.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProbationConfirmationAlert>> GetPendingAlertsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProbationConfirmationAlerts
            .AsNoTracking()
            .Where(p => p.AlertSentOn == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProbationConfirmationAlert> AddAsync(ProbationConfirmationAlert alert, CancellationToken cancellationToken = default)
    {
        _context.ProbationConfirmationAlerts.Add(alert);
        await _context.SaveChangesAsync(cancellationToken);
        return alert;
    }

    public async Task UpdateAsync(ProbationConfirmationAlert alert, CancellationToken cancellationToken = default)
    {
        _context.Entry(alert).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long probationId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.ProbationConfirmationAlerts.FindAsync(new object[] { probationId }, cancellationToken);
        if (entity is not null)
        {
            _context.ProbationConfirmationAlerts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
