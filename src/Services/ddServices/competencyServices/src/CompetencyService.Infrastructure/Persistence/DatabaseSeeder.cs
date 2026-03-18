using CompetencyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CompetencyService.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CompetencyDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CompetencyDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.CompetencyMasters.AnyAsync())
            {
                logger.LogInformation("Seeding competency master data...");

                var competencies = new[]
                {
                    CompetencyMaster.Create(1001, "Leadership & Influence",
                        new DateTime(2024, 1, 1), "CORE", null, null, null,
                        "Inspires and motivates teams", "Fails to provide direction", null),
                    CompetencyMaster.Create(1002, "Communication Skills",
                        new DateTime(2024, 1, 1), "CORE", null, null, null,
                        "Articulates ideas clearly; listens actively", "Poor listening skills", null),
                    CompetencyMaster.Create(1003, "Problem Solving & Analysis",
                        new DateTime(2024, 1, 1), "CORE", null, null, null,
                        "Identifies root causes; devises effective solutions", "Applies superficial fixes", null),
                    CompetencyMaster.Create(1004, "Teamwork & Collaboration",
                        new DateTime(2024, 1, 1), "CORE", null, null, null,
                        "Fosters cooperation; supports team goals", "Works in silos", null),
                    CompetencyMaster.Create(1005, "Customer Focus",
                        new DateTime(2024, 1, 1), "FUNC", null, null, null,
                        "Understands customer needs; delivers value", "Ignores feedback", null)
                };

                await context.CompetencyMasters.AddRangeAsync(competencies);
                competencies.ToList().ForEach(c => c.ClearDomainEvents()); // Suppress events during seed
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} competencies.", competencies.Length);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the database.");
            throw;
        }
    }
}
