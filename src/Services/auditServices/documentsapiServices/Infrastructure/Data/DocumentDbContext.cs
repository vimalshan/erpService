using DocumentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Infrastructure.Data;

public class DocumentDbContext : DbContext
{
    public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options) { }

    public DbSet<Document> Documents => Set<Document>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>(e =>
        {
            e.ToTable("Documents");
            e.HasKey(x => x.Id);
            e.Property(x => x.DocumentId).IsRequired();
            e.HasIndex(x => x.DocumentId).IsUnique();
            e.Property(x => x.FileName).HasMaxLength(500).IsRequired();
            e.Property(x => x.ContentType).HasMaxLength(200).IsRequired();
            e.Property(x => x.StoragePath).HasMaxLength(1000);
            e.Property(x => x.Category).HasMaxLength(100);
            e.Property(x => x.UploadedBy).HasMaxLength(200);
            e.Property(x => x.UploadedDate).HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.IsDeleted).HasDefaultValue(false);
            e.HasIndex(x => x.AuditId);
            e.HasIndex(x => x.FindingId);
            e.HasIndex(x => x.CertificateId);
            e.HasIndex(x => x.ContractId);
            e.HasIndex(x => x.IsDeleted);
        });
    }
}

public static class DocumentDbContextSeed
{
    public static async Task SeedAsync(DocumentDbContext db)
    {
        if (await db.Documents.AnyAsync()) return;

        var now = DateTime.UtcNow;
        db.Documents.AddRange(
            new Document { DocumentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), FileName = "AuditReport-2025-Q1.pdf", ContentType = "application/pdf", FileSize = 245678, Category = "Audit", AuditId = 1, UploadedBy = "admin@dnv.com", UploadedDate = now.AddDays(-30) },
            new Document { DocumentId = Guid.Parse("22222222-2222-2222-2222-222222222222"), FileName = "Finding-CAPA-001.docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", FileSize = 56789, Category = "Finding", FindingId = 101, UploadedBy = "auditor@dnv.com", UploadedDate = now.AddDays(-20) },
            new Document { DocumentId = Guid.Parse("33333333-3333-3333-3333-333333333333"), FileName = "ISO9001-Certificate.pdf", ContentType = "application/pdf", FileSize = 89012, Category = "Certificate", CertificateId = 201, UploadedBy = "admin@dnv.com", UploadedDate = now.AddDays(-15) },
            new Document { DocumentId = Guid.Parse("44444444-4444-4444-4444-444444444444"), FileName = "Contract-Renewal-2026.pdf", ContentType = "application/pdf", FileSize = 134567, Category = "Contract", ContractId = 301, UploadedBy = "admin@dnv.com", UploadedDate = now.AddDays(-10) },
            new Document { DocumentId = Guid.Parse("55555555-5555-5555-5555-555555555555"), FileName = "Evidence-Photo.jpg", ContentType = "image/jpeg", FileSize = 678901, Category = "Evidence", AuditId = 1, FindingId = 101, UploadedBy = "auditor@dnv.com", UploadedDate = now.AddDays(-5) }
        );
        await db.SaveChangesAsync();
    }
}
