using EmployeeTransactionsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeTransactionsService.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitialiseAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeTransactionsDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<EmployeeTransactionsDbContext>>();

        try
        {
            await dbContext.Database.MigrateAsync();
            await SeedAsync(dbContext, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the employee transactions database.");
            throw;
        }
    }

    private static async Task SeedAsync(EmployeeTransactionsDbContext dbContext, ILogger logger)
    {
        if (!await dbContext.Employees.AnyAsync())
        {
            var employee = EmployeeMain.Create(
                1,
                100001,
                DateTime.UtcNow.Date,
                "HQ1",
                101,
                9001,
                "Management Trainee",
                Domain.ValueObjects.EmployeeName.Create("Seed", null, "Employee"),
                "M",
                new DateTime(1995, 1, 1),
                "A",
                Domain.ValueObjects.EmailAddress.CreateOptional("seed.employee@corp.local"),
                Domain.ValueObjects.EmailAddress.CreateOptional("seed.employee@gmail.com"),
                "9999999999",
                "GEN",
                DateTime.UtcNow.Date.AddMonths(6),
                1,
                1);
            dbContext.Employees.Add(employee);
            dbContext.EmployeeGrades.Add(EmployeeGrade.Create(1, 1, 101, DateTime.UtcNow.Date, 1, "Y"));
            dbContext.EmployeeProbations.Add(EmployeeProbation.CreateInitial(1, 1, DateTime.UtcNow.Date.AddMonths(6)));
        }

        if (!await dbContext.AlertGroups.AnyAsync())
        {
            var alertGroup = AlertGroup.Create(1, "Default Reporting Group", "R", 1);
            alertGroup.AddRecipient(1, 1, "seed.employee@corp.local", 1, 1, null, DateTime.UtcNow.Date, null, 1);
            dbContext.AlertGroups.Add(alertGroup);
        }

        await dbContext.SaveChangesAsync();
        logger.LogInformation("EmployeeTransactionsService seed data applied.");
    }
}