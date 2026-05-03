using AuditService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditService.Infrastructure.Data;

public static class AuditDataSeeder
{
    public static async Task SeedAsync(AuditDomainDbContext db, ILogger logger, CancellationToken ct = default)
    {
        await db.Database.MigrateAsync(ct);

        await EnsureFindingsTableAsync(db, ct);

        if (!await db.Sites.AnyAsync(ct))
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SET IDENTITY_INSERT Sites ON;");
            var rows = new[]
            {
                (1, "Headquarters - New York",       "123 Main St, New York, USA"),
                (2, "Manufacturing Plant - Chicago", "456 Industrial Ave, Chicago, USA"),
                (3, "Warehouse - Los Angeles",       "789 Logistics Blvd, Los Angeles, USA"),
                (4, "London Office",                 "10 Downing Lane, London, UK"),
                (5, "Berlin Branch",                 "50 Unter den Linden, Berlin, Germany"),
                (6, "Tokyo Operations",              "1-1-1 Marunouchi, Tokyo, Japan")
            };
            foreach (var (id, name, loc) in rows)
            {
                sb.AppendLine($"INSERT INTO Sites (SiteId, SiteName, Location) VALUES ({id}, N'{name.Replace("'", "''")}', N'{loc.Replace("'", "''")}');");
            }
            sb.AppendLine("SET IDENTITY_INSERT Sites OFF;");
            await db.Database.ExecuteSqlRawAsync(sb.ToString(), ct);
            logger.LogInformation("Seeded {Count} Sites", rows.Length);
        }

        if (!await db.AuditTypes.AnyAsync(ct))
        {
            var now = DateTime.UtcNow;
            db.AuditTypes.AddRange(
                new AuditType { AuditTypeId = 1, AuditTypeName = "Internal",   AuditTypeCode = "INT", IsActive = true, CreatedDate = now, ModifiedDate = now, DisplayOrder = 1 },
                new AuditType { AuditTypeId = 2, AuditTypeName = "External",   AuditTypeCode = "EXT", IsActive = true, CreatedDate = now, ModifiedDate = now, DisplayOrder = 2 },
                new AuditType { AuditTypeId = 3, AuditTypeName = "Surveillance", AuditTypeCode = "SUR", IsActive = true, CreatedDate = now, ModifiedDate = now, DisplayOrder = 3 }
            );
            await db.SaveChangesAsync(ct);
            logger.LogInformation("Seeded {Count} AuditTypes", 3);
        }

        if (!await db.Audits.AnyAsync(ct))
        {
            var seed = new[]
            {
                new { AuditId = 1392092, CompanyId = 12345, Status = "Completed",  StartDate = new DateTime(2025,9,1),  EndDate = new DateTime(2025,9,10), Lead = "John Doe",   Type = "Internal", SiteIds = new[]{171912},                ServiceIds = new[]{1} },
                new { AuditId = 1392093, CompanyId = 12346, Status = "InProgress", StartDate = new DateTime(2025,9,5),  EndDate = new DateTime(2025,9,15), Lead = "Jane Smith", Type = "External", SiteIds = new[]{171913,171914},        ServiceIds = new[]{2,3} },
                new { AuditId = 1392094, CompanyId = 12347, Status = "Scheduled",  StartDate = new DateTime(2025,9,10), EndDate = new DateTime(2025,9,20), Lead = "Mike Brown", Type = "Internal", SiteIds = new[]{171915},                ServiceIds = new[]{4} },
            };

            var now = DateTime.UtcNow;
            foreach (var a in seed)
            {
                var audit = new Audit
                {
                    AuditId    = a.AuditId,
                    CompanyId  = a.CompanyId,
                    Status     = a.Status,
                    StartDate  = a.StartDate,
                    EndDate    = a.EndDate,
                    LeadAuditor= a.Lead,
                    Type       = a.Type,
                    Sites      = string.Join(",", a.SiteIds),
                    Services   = string.Join(",", a.ServiceIds.Select(id => $"Service{id}"))
                };
                foreach (var sid in a.SiteIds)
                    audit.AuditSites.Add(new AuditSite { SiteId = sid, IsActive = true, CreatedDate = now, ModifiedDate = now });
                foreach (var svc in a.ServiceIds)
                    audit.AuditServices.Add(new AuditServiceEntity { ServiceId = svc, IsActive = true, CreatedDate = now, ModifiedDate = now });
                audit.AuditTeamMembers.Add(new AuditTeamMember { UserId = 1, Role = "Lead Auditor", IsActive = true, CreatedDate = now, ModifiedDate = now });
                db.Audits.Add(audit);
            }
            await db.SaveChangesAsync(ct);
            logger.LogInformation("Seeded {Count} Audits with sites/services/team", seed.Length);
        }

        await SeedFindingsAsync(db, logger, ct);
    }

    private static async Task EnsureFindingsTableAsync(AuditDomainDbContext db, CancellationToken ct)
    {
        const string ddl = @"
IF OBJECT_ID(N'dbo.Findings', N'U') IS NULL
CREATE TABLE dbo.Findings (
    FindingId      INT IDENTITY(1,1) PRIMARY KEY,
    FindingNumber  NVARCHAR(50)  NOT NULL,
    AuditId        INT           NOT NULL,
    CompanyId      INT           NULL,
    SiteId         INT           NULL,
    Title          NVARCHAR(200) NULL,
    Status         NVARCHAR(50)  NULL,
    Category       NVARCHAR(100) NULL,
    OpenDate       DATETIME2     NULL,
    DueDate        DATETIME2     NULL,
    AcceptedDate   DATETIME2     NULL,
    ClosedDate     DATETIME2     NULL
);";
        await db.Database.ExecuteSqlRawAsync(ddl, ct);
    }

    private static async Task SeedFindingsAsync(AuditDomainDbContext db, ILogger logger, CancellationToken ct)
    {
        var existing = (int)(await db.Database
            .SqlQueryRaw<int>("SELECT COUNT(*) AS Value FROM Findings").ToListAsync(ct))[0];
        if (existing > 0) return;

        var rows = new[]
        {
            ("F-1-001", 1, 1, 1, "Document control inconsistency",     "Open",     "Documentation"),
            ("F-1-002", 1, 1, 2, "Calibration records incomplete",     "Closed",   "Process"),
            ("F-1-003", 1, 1, 3, "Training records missing for staff", "Open",     "Training"),
            ("F-2-001", 2, 1, 1, "Internal audit schedule overdue",    "Accepted", "Process"),
            ("F-6-001", 6, 2, 4, "Risk assessment outdated",            "Open",     "Risk"),
            ("F-6-002", 6, 2, 5, "Corrective action follow-up missing", "Closed",   "Corrective Action")
        };

        var openDate = DateTime.UtcNow.AddDays(-30);
        var dueDate = DateTime.UtcNow.AddDays(30);
        var closed = DateTime.UtcNow.AddDays(-1);
        foreach (var (num, auditId, companyId, siteId, title, status, category) in rows)
        {
            var closedSql = status == "Closed" ? $"'{closed:yyyy-MM-dd HH:mm:ss}'" : "NULL";
            var sql = $@"INSERT INTO Findings (FindingNumber, AuditId, CompanyId, SiteId, Title, Status, Category, OpenDate, DueDate, ClosedDate)
                         VALUES (N'{num}', {auditId}, {companyId}, {siteId}, N'{title.Replace("'", "''")}', N'{status}', N'{category}',
                                 '{openDate:yyyy-MM-dd HH:mm:ss}', '{dueDate:yyyy-MM-dd HH:mm:ss}', {closedSql});";
            await db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        logger.LogInformation("Seeded {Count} Findings", rows.Length);
    }
}
