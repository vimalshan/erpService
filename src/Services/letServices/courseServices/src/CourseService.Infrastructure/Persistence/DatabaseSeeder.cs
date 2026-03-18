using CourseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CourseService.Infrastructure.Persistence;

/// <summary>
/// Seeds initial data into the database on application startup.
/// </summary>
public class DatabaseSeeder(IServiceProvider serviceProvider, ILogger<DatabaseSeeder> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CourseDbContext>();

        try
        {
            // EnsureCreated is safe for LocalDB dev — creates the DB if it doesn't exist
            await db.Database.EnsureCreatedAsync(ct);
            await db.Database.MigrateAsync(ct);
            logger.LogInformation("Database migrations applied successfully.");

            // Seed only if no courses exist
            if (!await db.Courses.AnyAsync(ct))
            {
                await SeedCoursesAsync(db, ct);
                logger.LogInformation("Seed data inserted successfully.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database. The API will continue without seed data.");
        }
    }

    private static async Task SeedCoursesAsync(CourseDbContext db, CancellationToken ct)
    {
        var sql = """
            INSERT INTO COURSE_MAST (CR_CRS_ID, CR_CRS_TYP, CR_CRS_DES, CR_EFF_DAT, CR_CLS_DAT, CR_OBJ_DES,
                CR_LOC_COD, CR_ADD_LN1, CR_ADD_LN2, CR_ADD_LN3, CR_PIN_COD, CR_PHN_NUM,
                CR_STR_DAT, CR_END_DAT, CR_LST_DAT, CR_NO_DYS, CR_TRN_TYP)
            VALUES
                (1001, 'I', 'Introduction to ERP Systems', '2026-04-01', '2026-03-25', 'Learn ERP fundamentals',
                 'M', 'HQ Building, Floor 3', 'Sector 15', 'New Delhi', 110001, '+91-11-12345678',
                 '2026-04-01', '2026-04-03', '2026-03-20', 3, 'C'),
                (1002, 'E', 'Advanced Project Management', '2026-05-01', '2026-04-25', 'Master project management',
                 'B', '5th Avenue, Suite 200', 'Business Park', 'Mumbai', 400001, '+91-22-98765432',
                 '2026-05-05', '2026-05-07', '2026-04-20', 3, 'W'),
                (1003, 'O', 'Digital Transformation Workshop', '2026-06-01', '2026-05-20', 'Navigate digital change',
                 'N', 'Online Platform', '', '', 0, '+91-0000-000000',
                 '2026-06-10', '2026-06-12', '2026-05-15', 3, 'O');
            """;

        await db.Database.ExecuteSqlRawAsync(sql, ct);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
