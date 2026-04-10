using TaskTransactional.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TaskTransactional.Infrastructure.Persistence.Seed;

public static class ComplaintDbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ComplaintDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ComplaintDbContext>>();

        try
        {
            await db.Database.MigrateAsync();

            if (!await db.ComplaintMains.AnyAsync())
            {
                await db.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO COMPL_MAIN (CM_UNIT_CODE, CM_GROUPID, CM_GROUP_NAME, CM_GROUP_DESC, CM_GROUP_SRC, CM_SUBMIT, CM_REG_DATE)
                    VALUES ('001', '1', 'General Complaints', 'General complaint group', 1, 'Y', GETDATE());

                    INSERT INTO COMPL_MAIN (CM_UNIT_CODE, CM_GROUPID, CM_GROUP_NAME, CM_GROUP_DESC, CM_GROUP_SRC, CM_SUBMIT, CM_REG_DATE)
                    VALUES ('001', '2', 'NCR Group', 'Non-conformance reports', 2, 'Y', GETDATE());

                    INSERT INTO COMPL_MAIN (CM_UNIT_CODE, CM_GROUPID, CM_GROUP_NAME, CM_GROUP_DESC, CM_GROUP_SRC, CM_SUBMIT, CM_REG_DATE)
                    VALUES ('002', '3', 'Maintenance Issues', 'Equipment maintenance complaints', 1, 'Y', GETDATE());
                ");
            }

            if (!await db.ComplaintDetails.AnyAsync())
            {
                await db.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO COMPL_DET (CD_TICKET_NUM, CD_GROUPID, CD_TYPE, CD_LOCATION, CD_DEPARTMENT, CD_PROCESS, CD_SUBJECT, CD_DESCRIPTION, CD_NCR, CD_TARGET_DATE)
                    VALUES (1, 1, 1, 1, 1, 1, 'Equipment Failure', 'Compressor unit not responding', 'N', '2025-07-01');

                    INSERT INTO COMPL_DET (CD_TICKET_NUM, CD_GROUPID, CD_TYPE, CD_LOCATION, CD_DEPARTMENT, CD_PROCESS, CD_SUBJECT, CD_DESCRIPTION, CD_NCR, CD_TARGET_DATE)
                    VALUES (2, 1, 2, 1, 2, 1, 'Quality Issue', 'Product batch failed inspection', 'Y', '2025-06-30');
                ");
            }

            if (!await db.ComplaintActions.AnyAsync())
            {
                await db.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO COMPL_ACTION (CA_ACTION_NUM, CA_TASK_NUM, CA_TRG_DAT, CA_CUR_ESCLEVEL)
                    VALUES (1, 1, GETDATE(), 0);

                    INSERT INTO COMPL_ACTION (CA_ACTION_NUM, CA_TASK_NUM, CA_TRG_DAT, CA_CUR_ESCLEVEL)
                    VALUES (2, 2, GETDATE(), 0);
                ");
            }

            if (!await db.ComplaintHistories.AnyAsync())
            {
                await db.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO COMPL_HIST (CH_HISTORY_NUM, CH_ACTION_NUM, CH_SERIAL_NUM, CH_FROM, CH_TO, CH_ACTION_DATE, CH_ACTION_TYPE, CH_REMARKS)
                    VALUES (1, 1, 1, 'Open', 'New Ticket', GETDATE(), 'O', 'Equipment Failure reported');

                    INSERT INTO COMPL_HIST (CH_HISTORY_NUM, CH_ACTION_NUM, CH_SERIAL_NUM, CH_FROM, CH_TO, CH_ACTION_DATE, CH_ACTION_TYPE, CH_REMARKS)
                    VALUES (2, 2, 1, 'Open', 'New Ticket', GETDATE(), 'O', 'Quality Issue reported');
                ");
            }

            logger.LogInformation("Complaint database seeded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding complaint database");
        }
    }
}
