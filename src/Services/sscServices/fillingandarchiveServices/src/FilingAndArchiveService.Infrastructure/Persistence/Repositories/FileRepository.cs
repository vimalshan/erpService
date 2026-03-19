using FilingAndArchiveService.Domain.Entities;
using FilingAndArchiveService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilingAndArchiveService.Infrastructure.Persistence.Repositories;

public class FileRepository : IFileRepository
{
    private readonly ApplicationDbContext _context;

    public FileRepository(ApplicationDbContext context) => _context = context;

    public Task<FileMaster?> GetByIdAsync(long fileId, CancellationToken cancellationToken = default)
        => _context.FileMasters.AsNoTracking().FirstOrDefaultAsync(f => f.FileId == fileId, cancellationToken);

    public Task<FileMaster?> GetByFileNoAsync(string orgId, string fileNo, CancellationToken cancellationToken = default)
        => _context.FileMasters.AsNoTracking()
            .FirstOrDefaultAsync(f => f.FileOrgId == orgId && f.FileNo == fileNo, cancellationToken);

    public async Task<IEnumerable<FileMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.FileMasters.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<FileMaster>> GetByOrgAsync(string orgId, CancellationToken cancellationToken = default)
        => await _context.FileMasters.AsNoTracking()
            .Where(f => f.FileOrgId == orgId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<FileMaster>> GetByYearAsync(long year, CancellationToken cancellationToken = default)
        => await _context.FileMasters.AsNoTracking()
            .Where(f => f.FileYear == year)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(FileMaster file, CancellationToken cancellationToken = default)
        => await _context.FileMasters.AddAsync(file, cancellationToken);

    public Task UpdateAsync(FileMaster file, CancellationToken cancellationToken = default)
    {
        _context.FileMasters.Update(file);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long fileId, CancellationToken cancellationToken = default)
    {
        var file = await _context.FileMasters.FindAsync([fileId], cancellationToken);
        if (file is not null)
            _context.FileMasters.Remove(file);
    }

    public Task<bool> ExistsAsync(long fileId, CancellationToken cancellationToken = default)
        => _context.FileMasters.AnyAsync(f => f.FileId == fileId, cancellationToken);
}
