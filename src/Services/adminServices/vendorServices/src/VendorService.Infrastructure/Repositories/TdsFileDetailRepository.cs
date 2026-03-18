using Microsoft.EntityFrameworkCore;
using VendorService.Domain.Entities;
using VendorService.Domain.Interfaces;
using VendorService.Infrastructure.Data;

namespace VendorService.Infrastructure.Repositories;

public sealed class TdsFileDetailRepository : ITdsFileDetailRepository
{
    private readonly VendorDbContext _context;

    public TdsFileDetailRepository(VendorDbContext context)
    {
        _context = context;
    }

    public async Task<TdsFileDetail?> GetByIdAsync(long fileId, CancellationToken ct = default) =>
        await _context.TdsFileDetails.FirstOrDefaultAsync(f => f.FileId == fileId, ct);

    public async Task<IEnumerable<TdsFileDetail>> GetAllAsync(CancellationToken ct = default) =>
        await _context.TdsFileDetails.ToListAsync(ct);

    public async Task AddAsync(TdsFileDetail fileDetail, CancellationToken ct = default) =>
        await _context.TdsFileDetails.AddAsync(fileDetail, ct);

    public void Update(TdsFileDetail fileDetail) =>
        _context.TdsFileDetails.Update(fileDetail);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);
}
