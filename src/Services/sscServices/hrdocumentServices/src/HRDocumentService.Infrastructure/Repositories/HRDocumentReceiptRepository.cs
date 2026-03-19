using HRDocumentService.Domain.Entities;
using HRDocumentService.Domain.Interfaces;
using HRDocumentService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRDocumentService.Infrastructure.Repositories;

public sealed class HRDocumentReceiptRepository(HRDocumentDbContext context) : IHRDocumentReceiptRepository
{
    public async Task<HRDocumentReceipt?> GetByIdAsync(long recId, CancellationToken ct = default)
    {
        return await context.HRDocumentReceipts.FirstOrDefaultAsync(r => r.HRRecId == recId, ct);
    }

    public async Task<IReadOnlyList<HRDocumentReceipt>> GetByDocIdAsync(long docId, CancellationToken ct = default)
    {
        return await context.HRDocumentReceipts
            .Where(r => r.HRRecHRDocId == docId)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(HRDocumentReceipt receipt, CancellationToken ct = default)
    {
        await context.HRDocumentReceipts.AddAsync(receipt, ct);
    }
}
