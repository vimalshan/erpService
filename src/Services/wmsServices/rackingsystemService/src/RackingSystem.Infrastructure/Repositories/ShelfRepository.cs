using Microsoft.EntityFrameworkCore;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Interfaces;
using RackingSystem.Infrastructure.Persistence;

namespace RackingSystem.Infrastructure.Repositories;

public sealed class ShelfRepository : IShelfRepository
{
    private readonly ApplicationDbContext _context;
    public ShelfRepository(ApplicationDbContext context) => _context = context;

    public async Task<Shelf?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Shelves.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IEnumerable<Shelf>> GetByRackIdAsync(int rackId, CancellationToken ct = default) =>
        await _context.Shelves.Where(s => s.RackId == rackId && s.IsActive)
            .OrderBy(s => s.ShelfLevel).ThenBy(s => s.ShelfPosition).ToListAsync(ct);

    public async Task AddAsync(Shelf shelf, CancellationToken ct = default) =>
        await _context.Shelves.AddAsync(shelf, ct);

    public void Update(Shelf shelf) => _context.Shelves.Update(shelf);
    public void Remove(Shelf shelf) => _context.Shelves.Remove(shelf);
}
