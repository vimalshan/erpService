using HRDocumentService.Domain.Entities;
using HRDocumentService.Domain.Interfaces;
using HRDocumentService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRDocumentService.Infrastructure.Repositories;

public sealed class HRDocumentRepository(HRDocumentDbContext context) : IHRDocumentRepository
{
    public async Task<HRDocument?> GetByIdAsync(long docId, CancellationToken ct = default)
    {
        return await context.HRDocuments
            .Include(d => d.Files)
            .Include(d => d.Receipts)
            .FirstOrDefaultAsync(d => d.DocId == docId, ct);
    }

    public async Task<IReadOnlyList<HRDocument>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.HRDocuments
            .Include(d => d.Files)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<HRDocument>> GetByStatusAsync(string status, CancellationToken ct = default)
    {
        return await context.HRDocuments
            .Where(d => d.DocDocStatus == status)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<HRDocument>> GetByUserIdAsync(long userId, CancellationToken ct = default)
    {
        return await context.HRDocuments
            .Where(d => d.DocUserId == userId)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(HRDocument document, CancellationToken ct = default)
    {
        await context.HRDocuments.AddAsync(document, ct);
    }

    public void Update(HRDocument document)
    {
        context.HRDocuments.Update(document);
    }

    public void Delete(HRDocument document)
    {
        context.HRDocuments.Remove(document);
    }
}
