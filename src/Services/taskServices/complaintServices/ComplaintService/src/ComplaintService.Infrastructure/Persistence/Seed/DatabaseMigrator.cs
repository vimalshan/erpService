using ComplaintService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ComplaintService.Infrastructure.Persistence.Seed;

public static class DatabaseMigrator
{
    public static async Task MigrateAndSeedAsync(IHost app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ComplaintDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ComplaintDbContext>>();

        try
        {
            logger.LogInformation("Applying EF migrations...");
            await context.Database.MigrateAsync();

            // Seed stored procedures and functions
            await SeedStoredProceduresAsync(context, logger);
            logger.LogInformation("Database migration and seeding completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database migration failed.");
            throw;
        }
    }

    private static async Task SeedStoredProceduresAsync(ComplaintDbContext context, ILogger logger)
    {
        // Execute stored procedures SQL from embedded resource or raw SQL
        logger.LogInformation("Seeding stored procedures (usp_COMPLAINT_*)...");

        // Drop then recreate (CREATE OR ALTER is DDL and can't use IF EXISTS in same batch via EF)
        const string fnDrop = @"
            IF EXISTS (SELECT * FROM sys.objects WHERE name = 'fn_GetComplaintStatus' AND type = 'FN')
                DROP FUNCTION dbo.fn_GetComplaintStatus;";

        const string fnCreate = @"
            CREATE FUNCTION dbo.fn_GetComplaintStatus(@p_TicketNum DECIMAL(38))
            RETURNS VARCHAR(50)
            AS
            BEGIN
                DECLARE @Status   VARCHAR(50);
                DECLARE @TargetDate DATETIME2(3);
                DECLARE @HoursElapsed INT;

                SELECT @TargetDate = CD.CD_TARGET_DATE
                FROM   COMPL_DET CD
                WHERE  CD.CD_TICKET_NUM = @p_TicketNum;

                IF @TargetDate IS NULL
                    RETURN 'Unknown';

                SET @HoursElapsed = DATEDIFF(HOUR, @TargetDate, GETDATE());

                IF @HoursElapsed < 0
                    SET @Status = 'On Target';
                ELSE IF @HoursElapsed < 24
                    SET @Status = 'At Risk';
                ELSE IF @HoursElapsed < 48
                    SET @Status = 'Escalated - Level 1';
                ELSE
                    SET @Status = 'Escalated - Level 2+';

                RETURN ISNULL(@Status, 'Unknown');
            END;";

        await context.Database.ExecuteSqlRawAsync(fnDrop);
        await context.Database.ExecuteSqlRawAsync(fnCreate);
    }
}
