using AuditLogService.Domain.Entities;
using AuditLogService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditLogService.Infrastructure.Persistence;

public static class AuditLogDbSeeder
{
    public static async Task SeedAsync(AuditLogDbContext context, ILogger logger)
    {
        if (await context.AuditLogs.AnyAsync())
        {
            logger.LogInformation("Database already seeded.");
            return;
        }

        logger.LogInformation("Seeding AuditLog database...");

        var entries = new[]
        {
            AuditLogEntry.Create("Products", 1, "INSERT", "admin",
                null, "{\"Name\":\"Widget A\",\"Price\":9.99}"),
            AuditLogEntry.Create("Products", 1, "UPDATE", "admin",
                "{\"Price\":9.99}", "{\"Price\":12.99}"),
            AuditLogEntry.Create("Orders", 101, "INSERT", "system",
                null, "{\"OrderId\":101,\"Total\":250.00}"),
            AuditLogEntry.Create("Users", 5, "DELETE", "admin",
                "{\"Name\":\"John Doe\",\"Email\":\"john@example.com\"}", null),
            AuditLogEntry.Create("Inventory", 42, "UPDATE", "warehouse_mgr",
                "{\"Quantity\":100}", "{\"Quantity\":85}")
        };

        foreach (var entry in entries)
        {
            entry.ClearDomainEvents();
        }

        await context.AuditLogs.AddRangeAsync(entries);
        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT AuditLog OFF");
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} audit log entries.", entries.Length);
    }
}
