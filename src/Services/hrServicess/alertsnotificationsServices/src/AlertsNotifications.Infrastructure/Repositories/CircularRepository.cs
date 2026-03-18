using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Interfaces;
using AlertsNotifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlertsNotifications.Infrastructure.Repositories;

public class CircularRepository : ICircularRepository
{
    private readonly AlertsNotificationsDbContext _context;

    public CircularRepository(AlertsNotificationsDbContext context)
    {
        _context = context;
    }

    public async Task<Circular?> GetByIdAsync(long circularId, CancellationToken cancellationToken = default)
    {
        return await _context.Circulars
            .Include(c => c.Signatories)
            .Include(c => c.Template)
            .FirstOrDefaultAsync(c => c.CircularId == circularId, cancellationToken);
    }

    public async Task<IEnumerable<Circular>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Circulars.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Circular>> GetByStatusAsync(char status, CancellationToken cancellationToken = default)
    {
        return await _context.Circulars
            .AsNoTracking()
            .Where(c => c.CircularStatus == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Circular>> GetByOrgIdAsync(long orgId, CancellationToken cancellationToken = default)
    {
        return await _context.Circulars
            .AsNoTracking()
            .Where(c => c.CircularOrgId == orgId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Circular> AddAsync(Circular circular, CancellationToken cancellationToken = default)
    {
        _context.Circulars.Add(circular);
        await _context.SaveChangesAsync(cancellationToken);
        return circular;
    }

    public async Task UpdateAsync(Circular circular, CancellationToken cancellationToken = default)
    {
        _context.Entry(circular).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long circularId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Circulars.FindAsync(new object[] { circularId }, cancellationToken);
        if (entity is not null)
        {
            _context.Circulars.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
