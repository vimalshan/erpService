using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Interfaces;
using AlertsNotifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlertsNotifications.Infrastructure.Repositories;

public class CircularTemplateRepository : ICircularTemplateRepository
{
    private readonly AlertsNotificationsDbContext _context;

    public CircularTemplateRepository(AlertsNotificationsDbContext context)
    {
        _context = context;
    }

    public async Task<CircularTemplate?> GetByIdAsync(long templateId, CancellationToken cancellationToken = default)
    {
        return await _context.CircularTemplates.FindAsync(new object[] { templateId }, cancellationToken);
    }

    public async Task<IEnumerable<CircularTemplate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CircularTemplates.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CircularTemplate>> GetByTypeIdAsync(long typeId, CancellationToken cancellationToken = default)
    {
        return await _context.CircularTemplates
            .AsNoTracking()
            .Where(t => t.CircularTemplateTypeId == typeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<CircularTemplate> AddAsync(CircularTemplate template, CancellationToken cancellationToken = default)
    {
        _context.CircularTemplates.Add(template);
        await _context.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task UpdateAsync(CircularTemplate template, CancellationToken cancellationToken = default)
    {
        _context.Entry(template).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long templateId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.CircularTemplates.FindAsync(new object[] { templateId }, cancellationToken);
        if (entity is not null)
        {
            _context.CircularTemplates.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
