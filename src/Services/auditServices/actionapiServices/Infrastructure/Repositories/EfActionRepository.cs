using ActionService.Domain.Entities;
using ActionService.Domain.Interfaces;
using ActionService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActionService.Infrastructure.Repositories;

public class EfActionRepository : IActionRepository
{
    private readonly ActionDbContext _context;

    public EfActionRepository(ActionDbContext context) => _context = context;

    public async Task<ActionItem?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Actions.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<ActionItem>> GetAllAsync(CancellationToken ct = default)
        => await _context.Actions.ToListAsync(ct);

    public async Task<IEnumerable<ActionItem>> GetByEntityAsync(string entityType, int entityId, CancellationToken ct = default)
        => await _context.Actions
            .Where(a => a.EntityType == entityType && a.EntityId == entityId)
            .ToListAsync(ct);

    public async Task<ActionItem> AddAsync(ActionItem entity, CancellationToken ct = default)
    {
        _context.Actions.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(ActionItem entity, CancellationToken ct = default)
    {
        _context.Actions.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _context.Actions.FindAsync(new object[] { id }, ct);
        if (entity is not null)
        {
            _context.Actions.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}
