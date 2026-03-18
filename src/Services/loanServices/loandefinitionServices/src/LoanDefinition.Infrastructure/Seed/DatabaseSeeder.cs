using LoanDefinition.Domain.Entities;
using LoanDefinition.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LoanDefinition.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LoanDefinitionDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<LoanDefinitionDbContext>>();

        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrated successfully");

            if (!await context.LoanTypeMasters.AnyAsync())
            {
                var loanTypes = new[]
                {
                    LoanTypeMaster.Create(1, "Personal Loan", "PER", 1),
                    LoanTypeMaster.Create(2, "Housing Loan", "HSG", 1),
                    LoanTypeMaster.Create(3, "Vehicle Loan", "VEH", 1),
                    LoanTypeMaster.Create(4, "Education Loan", "EDU", 1),
                    LoanTypeMaster.Create(5, "Festival Advance", "FES", 1),
                };

                // Clear domain events from seed data
                foreach (var lt in loanTypes) lt.ClearDomainEvents();

                await context.LoanTypeMasters.AddRangeAsync(loanTypes);
                await context.SaveChangesAsync();

                logger.LogInformation("Seeded {Count} loan types", loanTypes.Length);
            }

            if (!await context.LoanFestivals.AnyAsync())
            {
                var festivals = new[]
                {
                    LoanFestival.Create(1, "Diwali Festival Advance", new DateTime(2026, 10, 1), new DateTime(2026, 11, 15), 1),
                    LoanFestival.Create(2, "Christmas Festival Advance", new DateTime(2026, 12, 1), new DateTime(2026, 12, 31), 1),
                    LoanFestival.Create(3, "Eid Festival Advance", new DateTime(2026, 3, 1), new DateTime(2026, 4, 15), 1),
                };

                foreach (var f in festivals) f.ClearDomainEvents();

                await context.LoanFestivals.AddRangeAsync(festivals);
                await context.SaveChangesAsync();

                logger.LogInformation("Seeded {Count} festivals", festivals.Length);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database");
            throw;
        }
    }
}
