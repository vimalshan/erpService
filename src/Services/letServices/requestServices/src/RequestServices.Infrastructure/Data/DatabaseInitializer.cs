using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RequestServices.Domain.Entities;
using RequestServices.Infrastructure.Data;

namespace RequestServices.Infrastructure.Data;

/// <summary>Runs EF migrations and seeds initial reference data on startup.</summary>
public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, ILogger logger)
    {
        using var scope  = serviceProvider.CreateScope();
        var context      = scope.ServiceProvider.GetRequiredService<RequestDbContext>();

        logger.LogInformation("Applying EF migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Migrations applied.");

        if (!await context.RequestMain.AnyAsync())
        {
            logger.LogInformation("Seeding initial request data...");
            await SeedAsync(context);
            logger.LogInformation("Seed complete.");
        }
    }

    private static async Task SeedAsync(RequestDbContext context)
    {
        var main = RequestMain.Create(
            requestId:      100001,
            employeeUser:   "EMP001",
            requestDate:    new DateTime(2026, 3, 14, 8, 0, 0),
            supervisorUser: "SUP001");

        await context.RequestMain.AddAsync(main);

        var sub = RequestSub.Create(
            requestId:          100001,
            serialNumber:       100001,
            requestDate:        new DateTime(2026, 3, 14, 8, 0, 0),
            statusCode:         'P',
            trainingNeed:       "Advanced C# and .NET Development",
            courseId:           5001,
            startDate:          new DateTime(2026, 4, 1),
            endDate:            new DateTime(2026, 4, 5),
            supervisorUser:     "SUP001",
            enteredUser:        "EMP001",
            businessBenefit:    "Improved development velocity and code quality",
            expectedCompetency: "Expert-level .NET proficiency",
            courseDescription:  "Advanced .NET 8 with C# features, CQRS and Clean Architecture");

        await context.RequestSub.AddAsync(sub);
        await context.SaveChangesAsync();
    }
}
