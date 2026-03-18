using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migration completed successfully.");

            // Seed is handled by SQL migration scripts to respect the existing DB schema.
            // No additional seed needed for production HRDB tables.
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            throw;
        }
    }
}

/// <summary>Seed data SQL script that can be run separately for initial data setup.</summary>
public static class SeedDataScript
{
    public const string Script = """
        -- Seed initial employee career record (example)
        -- INSERT INTO EMPLOYEE_CAREER (CAREER_ID, CAREER_EMP_SYSID, CAREER_BUSINESS, CAREER_UNIT, CAREER_EMPNO, CAREER_GRADE, CAREER_DESIGNATION, CAREER_MODIFIEDBY, CAREER_MODIFIEDON)
        -- VALUES (1, 1001, 'CORP     ', 'HQ ', 'EMP001', 1, 'Software Engineer', 1, GETUTCDATE());
        SELECT 1;
        """;
}
