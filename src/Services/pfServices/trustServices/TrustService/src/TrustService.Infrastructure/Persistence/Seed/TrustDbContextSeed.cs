using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrustService.Domain.Entities;
using TrustService.Domain.ValueObjects;

namespace TrustService.Infrastructure.Persistence.Seed;

public static class TrustDbContextSeed
{
    public static async Task SeedAsync(TrustDbContext context, ILogger logger)
    {
        if (await context.TrustMasters.AnyAsync())
        {
            logger.LogInformation("Trust database already seeded.");
            return;
        }

        logger.LogInformation("Seeding Trust database...");

        var trust1 = TrustMaster.Create(
            "T01", "Main Trust Fund", "PF1",
            new DateTime(2024, 01, 01),
            Address.Create("123 Main Street", "Suite 100", null, "Mumbai", "Maharashtra", "400001", "India"),
            ContactInfo.Create("022-12345678", "022-12345679", "maintrust@example.com"),
            "John Registrar", "9876543210");

        trust1.AddFundType("EPF", "Employee PF", "EPF");
        trust1.AddFundType("PPF", "Public PF", "PPF");
        trust1.AddRole(1, "ADM", "admin01", 1001);
        trust1.AddUnit("HQ1", "Head Office", "HeadOffice", "123 Main Street", "Suite 100", "Mumbai", "Maharashtra");

        var trust2 = TrustMaster.Create(
            "T02", "Regional Trust Fund", "PF2",
            new DateTime(2024, 06, 01),
            Address.Create("456 Park Avenue", null, null, "Delhi", "Delhi", "110001", "India"),
            ContactInfo.Create("011-87654321", null, "regional@example.com"),
            "Jane Registrar", "9123456780");

        trust2.AddFundType("GPF", "General PF", "GPF");
        trust2.AddRole(1, "ADM", "admin02", 1002);

        // Clear domain events from seed entities since these are initial seeds
        trust1.ClearDomainEvents();
        trust2.ClearDomainEvents();

        context.TrustMasters.AddRange(trust1, trust2);
        await context.Database.EnsureCreatedAsync();
        await context.SaveChangesAsync();

        logger.LogInformation("Trust database seeded successfully with {Count} trusts.", 2);
    }
}
