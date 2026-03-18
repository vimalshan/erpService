using Microsoft.EntityFrameworkCore;
using CardManagement.Domain.Entities;
using CardManagement.Domain.Interfaces;

namespace CardManagement.Infrastructure.Persistence.Repositories;

public class CanteenCardMapRepository : ICanteenCardMapRepository
{
    private readonly ApplicationDbContext _context;

    public CanteenCardMapRepository(ApplicationDbContext context) => _context = context;

    public async Task<CanteenCardMap?> GetByIdAsync(decimal sysId, CancellationToken ct = default)
        => await _context.CanteenCardMaps.FirstOrDefaultAsync(x => x.SysId == sysId, ct);

    public async Task<IEnumerable<CanteenCardMap>> GetByCanteenUnitAsync(long canteenUnit, CancellationToken ct = default)
        => await _context.CanteenCardMaps.Where(x => x.CanteenUnit == canteenUnit).ToListAsync(ct);

    public async Task<IEnumerable<CanteenCardMap>> GetActiveByCanteenUnitAsync(long canteenUnit, CancellationToken ct = default)
        => await _context.CanteenCardMaps
            .Where(x => x.CanteenUnit == canteenUnit && x.ClosingDate == null)
            .ToListAsync(ct);

    public async Task AddAsync(CanteenCardMap entity, CancellationToken ct = default)
        => await _context.CanteenCardMaps.AddAsync(entity, ct);

    public void Update(CanteenCardMap entity)
        => _context.CanteenCardMaps.Update(entity);
}
