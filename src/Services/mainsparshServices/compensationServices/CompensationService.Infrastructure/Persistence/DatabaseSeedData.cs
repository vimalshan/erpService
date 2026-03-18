using Microsoft.EntityFrameworkCore;
using CompensationService.Domain.Entities;
using CompensationService.Infrastructure.Persistence;

namespace CompensationService.Infrastructure.Persistence;

/// <summary>
/// Seed data for initial database setup
/// </summary>
public static class DatabaseSeedData
{
    public static async Task SeedAsync(CompensationDbContext context)
    {
        if (await context.CompensationGrades.AnyAsync())
            return; // Already seeded

        var grades = new List<CompensationGrade>
        {
            CompensationGrade.Create(
                "JR001",
                "Junior Executive",
                1,
                25000,
                10,
                5,
                new DateTime(2023, 1, 1),
                1),

            CompensationGrade.Create(
                "SR001",
                "Senior Executive",
                2,
                40000,
                15,
                8,
                new DateTime(2023, 1, 1),
                1),

            CompensationGrade.Create(
                "MG001",
                "Manager",
                3,
                60000,
                20,
                10,
                new DateTime(2023, 1, 1),
                1),

            CompensationGrade.Create(
                "SEN001",
                "Senior Manager",
                4,
                85000,
                25,
                12,
                new DateTime(2023, 1, 1),
                1),

            CompensationGrade.Create(
                "DIR001",
                "Director",
                5,
                120000,
                30,
                15,
                new DateTime(2023, 1, 1),
                1),
        };

        await context.CompensationGrades.AddRangeAsync(grades);
        await context.SaveChangesAsync();
    }
}
