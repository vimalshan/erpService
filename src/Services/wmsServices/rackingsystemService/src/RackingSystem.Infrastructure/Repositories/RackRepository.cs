using Microsoft.EntityFrameworkCore;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Interfaces;
using RackingSystem.Infrastructure.Persistence;

namespace RackingSystem.Infrastructure.Repositories;

public sealed class RackRepository : IRackRepository
{
    private readonly ApplicationDbContext _context;
    public RackRepository(ApplicationDbContext context) => _context = context;

    public async Task<Rack?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Racks.Include(r => r.Shelves).FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IEnumerable<Rack>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Racks.Include(r => r.Shelves).Where(r => r.IsActive).ToListAsync(ct);

    public async Task<IEnumerable<Rack>> GetByZoneIdAsync(int zoneId, CancellationToken ct = default) =>
        await _context.Racks.Include(r => r.Shelves)
            .Where(r => r.ZoneId == zoneId && r.IsActive).ToListAsync(ct);

    public async Task<bool> ExistsAsync(int zoneId, string code, CancellationToken ct = default) =>
        await _context.Racks.AnyAsync(r => r.ZoneId == zoneId && r.Code == code.ToUpperInvariant(), ct);

    public async Task AddAsync(Rack rack, CancellationToken ct = default) =>
        await _context.Racks.AddAsync(rack, ct);

    public void Update(Rack rack) => _context.Racks.Update(rack);
    public void Remove(Rack rack) => _context.Racks.Remove(rack);
}
