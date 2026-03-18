using AuditService.Domain.Entities;
using AuditService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditService.Infrastructure.Data.Seed;

public static class AuditDbSeedData
{
    public static async Task SeedAsync(AuditDbContext context, ILogger logger)
    {
        await context.Database.MigrateAsync();

        if (!await context.AuditYearMasters.AnyAsync())
        {
            context.AuditYearMasters.AddRange(
                new AuditYearMaster
                {
                    AymYearId = 1,
                    AymFrom = new DateTime(2025, 4, 1),
                    AymTo = new DateTime(2026, 3, 31),
                    AymLastModifiedBy = 1,
                    AymLastModifiedOn = DateTime.UtcNow
                },
                new AuditYearMaster
                {
                    AymYearId = 2,
                    AymFrom = new DateTime(2026, 4, 1),
                    AymTo = new DateTime(2027, 3, 31),
                    AymLastModifiedBy = 1,
                    AymLastModifiedOn = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded AuditYearMaster records.");
        }

        if (!await context.AuditProcessMasters.AnyAsync())
        {
            context.AuditProcessMasters.AddRange(
                new AuditProcessMaster { AuditProcessId = 1, AuditProcessName = "Financial Audit", AuditProcessCreatedBy = 1, AuditProcessCreatedOn = DateTime.UtcNow },
                new AuditProcessMaster { AuditProcessId = 2, AuditProcessName = "Operational Audit", AuditProcessCreatedBy = 1, AuditProcessCreatedOn = DateTime.UtcNow },
                new AuditProcessMaster { AuditProcessId = 3, AuditProcessName = "Compliance Audit", AuditProcessCreatedBy = 1, AuditProcessCreatedOn = DateTime.UtcNow },
                new AuditProcessMaster { AuditProcessId = 4, AuditProcessName = "IT Audit", AuditProcessCreatedBy = 1, AuditProcessCreatedOn = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded AuditProcessMaster records.");
        }

        if (!await context.AuditMasters.AnyAsync())
        {
            var audit = AuditMaster.Create(
                auditId: 1001,
                auditName: "FY26 Q1 Audit",
                auditUnit: 100,
                auditFrom: new DateTime(2026, 4, 1),
                auditTo: new DateTime(2026, 6, 30),
                defLocation: "Head Office",
                planFrom: new DateTime(2026, 3, 1),
                planTo: new DateTime(2026, 3, 31),
                createdBy: 1);
            audit.ClearDomainEvents();
            context.AuditMasters.Add(audit);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded AuditMaster records.");
        }
    }
}
