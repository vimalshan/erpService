using Microsoft.EntityFrameworkCore;
using VendorService.Domain.Entities;
using VendorService.Domain.Interfaces;
using VendorService.Infrastructure.Data;

namespace VendorService.Infrastructure.Repositories;

public sealed class TdsVendorRepository : ITdsVendorRepository
{
    private readonly VendorDbContext _context;

    public TdsVendorRepository(VendorDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TdsVendor>> GetAllAsync(CancellationToken ct = default) =>
        await _context.TdsVendors.ToListAsync(ct);

    public async Task<TdsVendor?> GetByVendorIdAsync(long vendorId, CancellationToken ct = default) =>
        await _context.TdsVendors.FirstOrDefaultAsync(t => t.VendorId == vendorId, ct);

    public async Task AddAsync(TdsVendor tdsVendor, CancellationToken ct = default) =>
        await _context.TdsVendors.AddAsync(tdsVendor, ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);
}
