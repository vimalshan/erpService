using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Interfaces;
using AlertsNotifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlertsNotifications.Infrastructure.Repositories;

public class CircularSignatoryRepository : ICircularSignatoryRepository
{
    private readonly AlertsNotificationsDbContext _context;

    public CircularSignatoryRepository(AlertsNotificationsDbContext context)
    {
        _context = context;
    }

    public async Task<CircularSignatory?> GetByIdAsync(long signatoryId, CancellationToken cancellationToken = default)
    {
        return await _context.CircularSignatories.FindAsync(new object[] { signatoryId }, cancellationToken);
    }

    public async Task<IEnumerable<CircularSignatory>> GetByUnitIdAsync(long unitId, CancellationToken cancellationToken = default)
    {
        return await _context.CircularSignatories
            .AsNoTracking()
            .Where(s => s.CircularSignatoryUnitId == unitId)
            .ToListAsync(cancellationToken);
    }

    public async Task<CircularSignatory> AddAsync(CircularSignatory signatory, CancellationToken cancellationToken = default)
    {
        _context.CircularSignatories.Add(signatory);
        await _context.SaveChangesAsync(cancellationToken);
        return signatory;
    }

    public async Task UpdateAsync(CircularSignatory signatory, CancellationToken cancellationToken = default)
    {
        _context.Entry(signatory).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long signatoryId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.CircularSignatories.FindAsync(new object[] { signatoryId }, cancellationToken);
        if (entity is not null)
        {
            _context.CircularSignatories.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
