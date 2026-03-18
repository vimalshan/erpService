using MasterService.Domain.Entities;
using MasterService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MasterService.Infrastructure.Persistence.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        await context.Database.MigrateAsync();

        if (!await context.Categories.AnyAsync())
        {
            context.Categories.AddRange(
                Category.Create("MGT", "Management"),
                Category.Create("TEC", "Technical"),
                Category.Create("OPS", "Operations"),
                Category.Create("FIN", "Finance"),
                Category.Create("HRD", "Human Resources"));
        }

        if (!await context.Goals.AnyAsync())
        {
            context.Goals.AddRange(
                Goal.Create("G01", "Increase Revenue"),
                Goal.Create("G02", "Reduce Costs"),
                Goal.Create("G03", "Improve Quality"),
                Goal.Create("G04", "Enhance Compliance"));
        }

        if (!await context.Modes.AnyAsync())
        {
            context.Modes.AddRange(
                Mode.Create("CLS", "Classroom"),
                Mode.Create("OJT", "On The Job Training"),
                Mode.Create("ONL", "Online"),
                Mode.Create("EXT", "External"));
        }

        if (!await context.Sources.AnyAsync())
        {
            context.Sources.AddRange(
                Source.Create("INT", "Internal"),
                Source.Create("EXT", "External"),
                Source.Create("ONL", "Online Platform"));
        }

        if (!await context.SkillGroups.AnyAsync())
        {
            context.SkillGroups.AddRange(
                SkillGroup.Create("TEC", "Technical"),
                SkillGroup.Create("SFT", "Soft Skills"),
                SkillGroup.Create("LDR", "Leadership"));
        }

        if (!await context.CompanyFinancialYears.AnyAsync())
        {
            context.CompanyFinancialYears.Add(
                CompanyFinancialYear.Create(1, new DateTime(2024, 1, 1), new DateTime(2024, 12, 31)));
            context.CompanyFinancialYears.Add(
                CompanyFinancialYear.Create(2, new DateTime(2025, 1, 1), new DateTime(2025, 12, 31)));
            context.CompanyFinancialYears.Add(
                CompanyFinancialYear.Create(3, new DateTime(2026, 1, 1), new DateTime(2026, 12, 31)));
        }

        await context.SaveChangesAsync();
        logger.LogInformation("Database seeded successfully.");
    }
}
