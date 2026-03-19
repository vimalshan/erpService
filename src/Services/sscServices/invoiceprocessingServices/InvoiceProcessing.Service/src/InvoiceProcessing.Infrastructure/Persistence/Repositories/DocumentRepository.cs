using InvoiceProcessing.Domain.Entities;
using InvoiceProcessing.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessing.Infrastructure.Persistence.Repositories;

public class DocumentRepository(InvoiceProcessingDbContext context) : IDocumentRepository
{
    public async Task<DocumentDetail?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await context.Documents
            .Include(d => d.OracleInvoiceDetails)
            .Include(d => d.OraclePaymentDetails)
            .Include(d => d.PoList)
            .Include(d => d.CostCenters)
            .Include(d => d.Attachments)
            .Include(d => d.ApAllocations)
            .Include(d => d.Correspondences)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }

    public async Task<IReadOnlyList<DocumentDetail>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Documents.AsNoTracking().ToListAsync(ct);
    }

    public async Task<IReadOnlyList<DocumentDetail>> GetByOrgIdAsync(string orgId, CancellationToken ct = default)
    {
        return await context.Documents.AsNoTracking()
            .Where(d => d.OrgId == orgId).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<DocumentDetail>> GetByStatusAsync(string status, CancellationToken ct = default)
    {
        return await context.Documents.AsNoTracking()
            .Where(d => d.DocumentStatus == status).ToListAsync(ct);
    }

    public async Task<DocumentDetail> AddAsync(DocumentDetail document, CancellationToken ct = default)
    {
        await context.Documents.AddAsync(document, ct);
        return document;
    }

    public Task UpdateAsync(DocumentDetail document, CancellationToken ct = default)
    {
        context.Documents.Update(document);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var doc = await context.Documents.FindAsync([id], ct);
        if (doc is not null) context.Documents.Remove(doc);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct = default)
    {
        return await context.Documents.AnyAsync(d => d.Id == id, ct);
    }

    public async Task<(IReadOnlyList<DocumentDetail> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? orgId = null, string? status = null, CancellationToken ct = default)
    {
        var query = context.Documents.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(orgId))
            query = query.Where(d => d.OrgId == orgId);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(d => d.DocumentStatus == status);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(d => d.CreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
