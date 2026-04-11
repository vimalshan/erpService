using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TransactionService.Infrastructure.Persistence.Seed;

public static class TransactionDbContextSeed
{
    public static async Task SeedAsync(TransactionDbContext context, ILogger logger)
    {
        try
        {
            if (context.Database.IsSqlServer())
                await context.Database.MigrateAsync();

            if (!await context.EmployeeJournalVouchers.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("""
                    INSERT INTO [dbo].[JVEMP_MAIN]
                        (JV_BATCHID, JV_TPID, JV_TYPE, JV_DATE, JV_EMPSYSID, JV_STATUS, JV_TRNTYPE, JV_NETAMT, JV_PAYUNITID, JV_CREATEDBY, JV_CREATEDON)
                    VALUES
                        (100001, 1, 'INV', '2025-01-15', 1001, 'P', 'ADV', '50000', 1, 1, GETDATE()),
                        (100002, 2, 'CRD', '2025-01-20', 1002, 'Y', 'EXP', '75000', 1, 1, GETDATE());

                    INSERT INTO [dbo].[JVEMP_SUB]
                        (JV_SUBID, JV_BATCHID, JV_SUBTYPE, JV_BU, JV_ACCODE, JV_SUBACC, JV_CCCODE, JV_PRODUCT, JV_DCFLAG, JV_TRNAMT, JV_IUTABU, JV_LOC, JV_REMARKS, JV_LINEFLAG)
                    VALUES
                        (200001, 100001, 'ADV', 'BU01', 'AC001', 'SA01', 'CC01', 'PRD01', 'D', '25000', 'IBU01', 'LOC01', 'Advance for Delhi trip', 'Y'),
                        (200002, 100001, 'ADV', 'BU01', 'AC001', 'SA01', 'CC01', 'PRD01', 'D', '25000', 'IBU01', 'LOC01', 'Advance part 2', 'Y'),
                        (200003, 100002, 'EXP', 'BU02', 'AC002', 'SA02', 'CC02', 'PRD02', 'C', '75000', 'IBU02', 'LOC02', 'Hotel expense Mumbai', 'Y');
                    """);

                logger.LogInformation("Seeded employee journal vouchers");
            }

            if (!await context.TravelBatches.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("""
                    INSERT INTO [dbo].[TRAVEL_BATCHMAIN]
                        (BATCH_ID, BATCH_ADMINID, BATCH_PAYUNITID, BATCH_BATCHDATE, BATCH_STATUS, BATCH_TYPE, BATCH_VENDORID, BATCH_CREATEDBY, BATCH_CREATEDON)
                    VALUES
                        ('B00001', 'ADM001', 'PU001', '2025-02-01', 'P', 'STD', 'V001', '1', GETDATE()),
                        ('B00002', 'ADM002', 'PU001', '2025-02-15', 'C', 'STD', 'V002', '1', GETDATE());

                    INSERT INTO [dbo].[TRAVEL_BATCHSUB]
                        (BATCHSUB_ID, BATCHSUB_BATCHID, BATCHSUB_BOOKCNFID, BATCHSUB_TPID, BATCHSUB_VENDORID, BATCHSUB_BASAMT, BATCHSUB_TOTAMT, BATCHSUB_TOTPAY, BATCHSUB_ADMREMARKS)
                    VALUES
                        ('300001', 'B00001', 'BK5001', 'TP01', 'V001', '80000', '80000', '80000', 'Hotel charges'),
                        ('300002', 'B00001', 'BK5001', 'TP02', 'V001', '70000', '70000', '70000', 'Transport charges'),
                        ('300003', 'B00002', 'BK5002', 'TP03', 'V002', '200000', '200000', '200000', 'Airfare charges');
                    """);

                logger.LogInformation("Seeded travel batches");
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "An error occurred during database seeding. Skipping seed data.");
        }
    }
}
