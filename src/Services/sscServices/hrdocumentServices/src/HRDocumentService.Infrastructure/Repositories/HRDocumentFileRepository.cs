using HRDocumentService.Domain.Entities;
using HRDocumentService.Domain.Interfaces;
using HRDocumentService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRDocumentService.Infrastructure.Repositories;

public sealed class HRDocumentFileRepository(HRDocumentDbContext context) : IHRDocumentFileRepository
{
    public async Task<HRDocumentFile?> GetByIdAsync(long fileId, CancellationToken ct = default)
    {
        return await context.HRDocumentFiles.FirstOrDefaultAsync(f => f.FileId == fileId, ct);
    }

    public async Task<IReadOnlyList<HRDocumentFile>> GetByDocIdAsync(long docId, CancellationToken ct = default)
    {
        return await context.HRDocumentFiles
            .Where(f => f.FileDocId == docId)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(HRDocumentFile file, CancellationToken ct = default)
    {
        await context.HRDocumentFiles.AddAsync(file, ct);
    }

    public void Delete(HRDocumentFile file)
    {
        context.HRDocumentFiles.Remove(file);
    }
}
