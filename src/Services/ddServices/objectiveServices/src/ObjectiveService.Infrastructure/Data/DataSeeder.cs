using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ObjectiveService.Domain.Entities;

namespace ObjectiveService.Infrastructure.Data;

/// <summary>
/// Seeds initial reference/demo data when the database is empty.
/// Called only in Development; production data is managed via migration scripts.
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Employees.AnyAsync())
        {
            logger.LogInformation("Database already contains data — skipping seed.");
            return;
        }

        logger.LogInformation("Seeding initial data...");

        var employees = new[]
        {
            new Employee("EMP001", 10001, 1001, "Operations"),
            new Employee("EMP002", 10002, 1002, "Finance"),
            new Employee("EMP003", 10003, 1003, "HR"),
            new Employee("EMP004", 10004, 1004, "IT"),
            new Employee("EMP005", 10005, 1005, "Sales")
        };

        await context.Employees.AddRangeAsync(employees);
        await context.SaveChangesAsync();

        // Seed sample control points (for EMP001 / year 2024)
        var cp1 = new ControlPoint(1001, 2024, "DD", 101, 1,
            "Process Efficiency", "Performance", "%", "80%", "95%", 1, 20);
        var cp2 = new ControlPoint(1001, 2024, "DD", 102, 2,
            "Customer Satisfaction", "Quality", "Score", "7/10", "9/10", 1, 15);
        var cp3 = new ControlPoint(1002, 2024, "DD", 201, 1,
            "Revenue Growth", "Financial", "%", "5%", "15%", 1, 25);

        await context.ControlPoints.AddRangeAsync(cp1, cp2, cp3);
        await context.SaveChangesAsync();

        // Seed sample goals
        var goal = new Goal("EMP001", 10001,
            new DateTime(2024, 1, 1), new DateTime(2024, 12, 31),
            2024001, 'D');

        goal.AddSubGoal(new GoalSubGoal(goal.Id, "Improve process turnaround",
            "10", "15", "Days", "Performance"));

        await context.Goals.AddAsync(goal);
        await context.SaveChangesAsync();

        logger.LogInformation("Seed data inserted successfully.");
    }
}
