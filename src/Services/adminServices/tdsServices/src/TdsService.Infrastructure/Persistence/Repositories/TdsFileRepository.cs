using Microsoft.EntityFrameworkCore;
using TdsService.Domain.Entities;
using TdsService.Domain.Repositories;
using TdsService.Domain.ValueObjects;

namespace TdsService.Infrastructure.Persistence.Repositories;

public sealed class TdsFileRepository : ITdsFileRepository
{
    private readonly TdsDbContext _context;

    public TdsFileRepository(TdsDbContext context) => _context = context;

    public async Task<TdsFile?> GetByIdAsync(long fileId, CancellationToken ct = default)
        => await _context.TdsFiles.FirstOrDefaultAsync(f => f.Id == fileId, ct);

    public async Task<IReadOnlyList<TdsFile>> GetAllAsync(CancellationToken ct = default)
        => await _context.TdsFiles.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<TdsFile>> GetByPanAsync(string panNo, CancellationToken ct = default)
        => await _context.TdsFiles
            .AsNoTracking()
            .Where(f => f.PanNumber != null && f.PanNumber.Value == panNo)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<TdsFile>> GetPendingEmailFilesAsync(CancellationToken ct = default)
        => await _context.TdsFiles
            .AsNoTracking()
            .Where(f => f.EmailStatus == EmailStatus.Pending)
            .ToListAsync(ct);

    public async Task AddAsync(TdsFile file, CancellationToken ct = default)
        => await _context.TdsFiles.AddAsync(file, ct);

    public void Update(TdsFile file)
        => _context.TdsFiles.Update(file);

    public void Remove(TdsFile file)
        => _context.TdsFiles.Remove(file);

    public async Task<bool> ExistsAsync(long fileId, CancellationToken ct = default)
        => await _context.TdsFiles.AnyAsync(f => f.Id == fileId, ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
