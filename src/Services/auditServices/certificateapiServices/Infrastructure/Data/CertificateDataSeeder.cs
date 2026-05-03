using CertificateService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CertificateService.Infrastructure.Data;

public static class CertificateDataSeeder
{
    public static async Task SeedAsync(CertificateDomainDbContext db, ILogger logger, CancellationToken ct = default)
    {
        await db.Database.MigrateAsync(ct);

        if (await db.Certificates.AnyAsync(ct))
        {
            logger.LogInformation("Certificates already seeded; skipping");
            return;
        }

        var now = DateTime.UtcNow;
        var certs = new List<Certificate>
        {
            new Certificate
            {
                CertificateNumber = "CERT-2024-001", CertificateName = "ISO 9001 - HQ NY",
                CompanyId = 1, SiteId = 1, ServiceId = 1,
                IssueDate = new DateTime(2024,3,20), ExpiryDate = new DateTime(2027,3,19),
                Status = "Active", CertificateType = "Initial", Scope = "Quality Management - Headquarters",
                IsActive = true, CreatedDate = now, ModifiedDate = now, CreatedBy = 1, ModifiedBy = 1, IssuedBy = 1,
                RevisionNumber = 1, AuditId = 1, Notes = "Initial certification",
                CertificateServices = { new CertificateServiceEntity { ServiceId = 1, IsActive = true, Scope = "ISO 9001" } },
                CertificateSites = { new CertificateSite { SiteId = 1, IsActive = true } },
                AdditionalScopes =
                {
                    new CertificateAdditionalScope { ScopeDescription = "Design and development of products", ScopeType = "Primary", IsActive = true, EffectiveDate = new DateTime(2024,3,20), ExpiryDate = new DateTime(2027,3,19) }
                }
            },
            new Certificate
            {
                CertificateNumber = "CERT-2024-002", CertificateName = "ISO 14001 - Chicago",
                CompanyId = 1, SiteId = 2, ServiceId = 2,
                IssueDate = new DateTime(2024,5,25), ExpiryDate = new DateTime(2027,5,24),
                Status = "Active", CertificateType = "Initial", Scope = "Environmental Management - Manufacturing",
                IsActive = true, CreatedDate = now, ModifiedDate = now, CreatedBy = 1, ModifiedBy = 1, IssuedBy = 1,
                RevisionNumber = 1, AuditId = 3,
                CertificateServices = { new CertificateServiceEntity { ServiceId = 2, IsActive = true, Scope = "ISO 14001" } },
                CertificateSites = { new CertificateSite { SiteId = 2, IsActive = true } }
            },
            new Certificate
            {
                CertificateNumber = "CERT-2025-003", CertificateName = "ISO 45001 - HQ NY",
                CompanyId = 1, SiteId = 1, ServiceId = 3,
                IssueDate = new DateTime(2025,7,15), ExpiryDate = new DateTime(2026,7,14),
                Status = "Active", CertificateType = "Surveillance", Scope = "Occupational Health & Safety",
                IsActive = true, CreatedDate = now, ModifiedDate = now, CreatedBy = 2, ModifiedBy = 2, IssuedBy = 2,
                RevisionNumber = 1,
                CertificateServices = { new CertificateServiceEntity { ServiceId = 3, IsActive = true, Scope = "ISO 45001" } },
                CertificateSites = { new CertificateSite { SiteId = 1, IsActive = true } }
            },
            new Certificate
            {
                CertificateNumber = "CERT-2023-004", CertificateName = "ISO 9001 - London",
                CompanyId = 2, SiteId = 4, ServiceId = 1,
                IssueDate = new DateTime(2023,9,15), ExpiryDate = new DateTime(2026,9,14),
                Status = "Active", CertificateType = "Initial", Scope = "Quality Management - London Operations",
                IsActive = true, CreatedDate = now, ModifiedDate = now, CreatedBy = 1, ModifiedBy = 1, IssuedBy = 1,
                RevisionNumber = 2,
                CertificateServices = { new CertificateServiceEntity { ServiceId = 1, IsActive = true, Scope = "ISO 9001" } },
                CertificateSites = { new CertificateSite { SiteId = 4, IsActive = true }, new CertificateSite { SiteId = 5, IsActive = true } }
            },
            new Certificate
            {
                CertificateNumber = "CERT-2022-005", CertificateName = "ISO 9001 - Tokyo (Expired)",
                CompanyId = 3, SiteId = 6, ServiceId = 1,
                IssueDate = new DateTime(2022,1,10), ExpiryDate = new DateTime(2025,1,9),
                Status = "Expired", CertificateType = "Initial", Scope = "Quality Management - Tokyo",
                IsActive = false, CreatedDate = now.AddYears(-4), ModifiedDate = now.AddYears(-1), CreatedBy = 1, ModifiedBy = 1, IssuedBy = 1,
                RevisionNumber = 1,
                CertificateServices = { new CertificateServiceEntity { ServiceId = 1, IsActive = false, Scope = "ISO 9001" } },
                CertificateSites = { new CertificateSite { SiteId = 6, IsActive = false } }
            }
        };

        db.Certificates.AddRange(certs);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Seeded {Count} Certificates", certs.Count);
    }
}
