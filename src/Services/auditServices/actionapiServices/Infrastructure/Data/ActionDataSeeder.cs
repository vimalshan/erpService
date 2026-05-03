using ActionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ActionService.Infrastructure.Data;

public static class ActionDataSeeder
{
    public static async Task SeedAsync(ActionDbContext db, ILogger logger, CancellationToken ct = default)
    {
        await db.Database.MigrateAsync(ct);

        if (await db.Actions.AnyAsync(ct))
        {
            logger.LogInformation("Actions already seeded; skipping");
            return;
        }

        var now = DateTime.UtcNow;
        var seed = new List<ActionItem>
        {
            new ActionItem
            {
                Action = "Review certificate expiry",
                DueDate = now.AddDays(7),
                HighPriority = true,
                Message = "Certificate ISO-9001 for Headquarters expires soon",
                Language = "en",
                Service = "ISO 9001",
                Site = "Headquarters - New York",
                EntityType = "Certificate",
                EntityId = 1001,
                Subject = "Certificate Expiry",
                SnowLink = "https://snow.example.com/RITM0001001"
            },
            new ActionItem
            {
                Action = "Close finding F-1-001",
                DueDate = now.AddDays(14),
                HighPriority = true,
                Message = "Document control inconsistency must be remediated",
                Language = "en",
                Service = "ISO 9001",
                Site = "Headquarters - New York",
                EntityType = "Finding",
                EntityId = 1,
                Subject = "Open Finding",
                SnowLink = "https://snow.example.com/RITM0001002"
            },
            new ActionItem
            {
                Action = "Submit calibration records",
                DueDate = now.AddDays(3),
                HighPriority = false,
                Message = "Calibration evidence pending for Manufacturing Plant",
                Language = "en",
                Service = "ISO 9001",
                Site = "Manufacturing Plant - Chicago",
                EntityType = "Finding",
                EntityId = 2,
                Subject = "Calibration",
                SnowLink = null
            },
            new ActionItem
            {
                Action = "Confirm surveillance audit schedule",
                DueDate = now.AddDays(30),
                HighPriority = false,
                Message = "Annual surveillance audit window confirmation",
                Language = "en",
                Service = "ISO 14001",
                Site = "Warehouse - Los Angeles",
                EntityType = "Schedule",
                EntityId = 4,
                Subject = "Surveillance Audit",
                SnowLink = null
            },
            new ActionItem
            {
                Action = "Upload training records",
                DueDate = now.AddDays(10),
                HighPriority = false,
                Message = "Staff training evidence missing",
                Language = "en",
                Service = "ISO 45001",
                Site = "London Office",
                EntityType = "Finding",
                EntityId = 3,
                Subject = "Training",
                SnowLink = null
            },
            new ActionItem
            {
                Action = "Acknowledge audit nonconformity",
                DueDate = now.AddDays(2),
                HighPriority = true,
                Message = "Internal audit schedule overdue",
                Language = "en",
                Service = "ISO 14001",
                Site = "Berlin Branch",
                EntityType = "Finding",
                EntityId = 4,
                Subject = "Nonconformity",
                SnowLink = "https://snow.example.com/RITM0001006"
            },
            new ActionItem
            {
                Action = "Renew certificate",
                DueDate = now.AddDays(60),
                HighPriority = false,
                Message = "Recertification audit due",
                Language = "en",
                Service = "ISO 9001",
                Site = "Tokyo Operations",
                EntityType = "Certificate",
                EntityId = 1002,
                Subject = "Recertification",
                SnowLink = null
            }
        };

        db.Actions.AddRange(seed);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Seeded {Count} Actions", seed.Count);
    }
}
