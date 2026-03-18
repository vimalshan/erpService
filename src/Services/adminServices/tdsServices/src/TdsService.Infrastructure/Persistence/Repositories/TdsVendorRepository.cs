using Microsoft.EntityFrameworkCore;
using TdsService.Domain.Entities;
using TdsService.Domain.Repositories;

namespace TdsService.Infrastructure.Persistence.Repositories;

public sealed class TdsVendorRepository : ITdsVendorRepository
{
    private readonly TdsDbContext _context;

    public TdsVendorRepository(TdsDbContext context) => _context = context;

    public async Task<TdsVendor?> GetByIdAsync(long vendorId, CancellationToken ct = default)
        => await _context.TdsVendors.FirstOrDefaultAsync(v => v.Id == vendorId, ct);

    public async Task<TdsVendor?> GetByPanAsync(string panNo, CancellationToken ct = default)
        => await _context.TdsVendors
            .FirstOrDefaultAsync(v => v.PanNumber != null && v.PanNumber.Value == panNo, ct);

    public async Task<IReadOnlyList<TdsVendor>> GetAllAsync(CancellationToken ct = default)
        => await _context.TdsVendors.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<TdsVendor>> GetPagedAsync(
        int page, int pageSize, CancellationToken ct = default)
        => await _context.TdsVendors
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task AddAsync(TdsVendor vendor, CancellationToken ct = default)
        => await _context.TdsVendors.AddAsync(vendor, ct);

    public void Update(TdsVendor vendor)
        => _context.TdsVendors.Update(vendor);

    public void Remove(TdsVendor vendor)
        => _context.TdsVendors.Remove(vendor);

    public async Task<bool> ExistsByPanAsync(string panNo, CancellationToken ct = default)
        => await _context.TdsVendors
            .AnyAsync(v => v.PanNumber != null && v.PanNumber.Value == panNo, ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
