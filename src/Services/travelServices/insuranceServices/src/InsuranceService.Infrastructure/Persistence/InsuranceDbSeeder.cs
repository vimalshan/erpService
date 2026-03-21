using InsuranceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InsuranceService.Infrastructure.Persistence;

public static class InsuranceDbSeeder
{
    public static async Task SeedAsync(InsuranceDbContext context, ILogger logger)
    {
        if (await context.TravelInsurances.AnyAsync()) return;

        logger.LogInformation("Seeding insurance data...");

        var seedData = new[]
        {
            TravelInsurance.Register("001", 1001, "LIF", "P12345678", null, "John Doe Jr.", null, "Initial life insurance"),
            TravelInsurance.Register("001", 1002, "MED", "P87654321", null, "Jane Smith", "Mark Smith", "Medical travel insurance"),
            TravelInsurance.Register("002", 2001, "TRV", "P11223344", null, "Alice Johnson", null, "Travel insurance policy"),
        };

        foreach (var insurance in seedData)
        {
            insurance.ClearDomainEvents();
        }

        context.TravelInsurances.AddRange(seedData);
        await context.Database.EnsureCreatedAsync();
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} insurance records.", seedData.Length);
    }
}
