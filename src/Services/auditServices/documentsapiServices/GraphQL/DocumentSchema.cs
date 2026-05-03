using DocumentService.Domain.Entities;
using DocumentService.Infrastructure.Data;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.GraphQL;

public class DocumentQuery
{
    [Authorize]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Document> GetDocuments([Service] DocumentDbContext db) =>
        db.Documents.Where(d => !d.IsDeleted);

    [Authorize]
    public Task<Document?> GetDocumentByIdAsync(Guid documentId, [Service] DocumentDbContext db) =>
        db.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId && !d.IsDeleted);

    [Authorize]
    public async Task<int> GetDocumentCountAsync([Service] DocumentDbContext db) =>
        await db.Documents.CountAsync(d => !d.IsDeleted);
}

public record CreateDocumentInput(
    string FileName,
    string ContentType,
    long FileSize,
    string? Category,
    int? AuditId,
    int? FindingId,
    int? CertificateId,
    int? ContractId);

public class DocumentMutation
{
    [Authorize]
    public async Task<Document> CreateDocumentAsync(CreateDocumentInput input, [Service] DocumentDbContext db)
    {
        var doc = new Document
        {
            FileName = input.FileName,
            ContentType = input.ContentType,
            FileSize = input.FileSize,
            Category = input.Category,
            AuditId = input.AuditId,
            FindingId = input.FindingId,
            CertificateId = input.CertificateId,
            ContractId = input.ContractId,
            UploadedDate = DateTime.UtcNow
        };
        db.Documents.Add(doc);
        await db.SaveChangesAsync();
        return doc;
    }

    [Authorize]
    public async Task<bool> DeleteDocumentAsync(Guid documentId, [Service] DocumentDbContext db)
    {
        var doc = await db.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
        if (doc is null || doc.IsDeleted) return false;
        doc.IsDeleted = true;
        doc.DeletedDate = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }
}
