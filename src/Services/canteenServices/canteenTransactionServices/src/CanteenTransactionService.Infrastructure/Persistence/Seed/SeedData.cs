using Microsoft.EntityFrameworkCore;
using CanteenTransactionService.Infrastructure.Persistence.EF;
using Microsoft.Extensions.Logging;

namespace CanteenTransactionService.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static async Task SeedAsync(CanteenTransactionDbContext db, ILogger logger)
    {
        if (await db.CanteenDacons.AnyAsync()) return;

        logger.LogInformation("Seeding CanteenTransaction database...");

        var dacon = Domain.Entities.CanteenDacon.Record(
            serialNumber: 1,
            companyCode: 1,
            employeeSysId: 1001,
            employeeType: "R",
            swipeDate: "2026-03-26 12:00:00",
            itemCode: 101,
            itemType: "M",
            employeeContribution: 25,
            employerContribution: 75,
            canteenNumber: "1",
            itemQuantity: 1,
            entryUser: 9999,
            gradeCategory: "A");

        dacon.ClearDomainEvents();
        await db.CanteenDacons.AddAsync(dacon);

        var availed = Domain.Entities.DailyAvailed.Create(
            serialNumber: 1,
            companyCode: 1,
            employeeSysId: 1001,
            employeeType: "R",
            swipeDate: "2026-03-26 12:00:00",
            itemCode: 101,
            itemType: "M",
            employeeContribution: 25,
            employerContribution: 75,
            canteenNumber: "1",
            itemQuantity: 1,
            entryUser: 9999,
            gradeCategory: "A");

        availed.ClearDomainEvents();
        await db.DailyAvaileds.AddAsync(availed);

        var batch = Domain.Entities.MisBatchSubmission.Create(
            companyCode: 1,
            employeeNumber: "EMP1001",
            swipeTime: new DateTime(2026, 3, 26, 12, 0, 0),
            itemCode: 101,
            itemQuantity: 1,
            batchDate: new DateTime(2026, 3, 26),
            batchNumber: 1,
            serialNumber: 1,
            canteenNumber: "1",
            gateNumber: "001");

        batch.ClearDomainEvents();
        await db.MisBatchSubmissions.AddAsync(batch);

        await db.SaveChangesAsync();
        logger.LogInformation("CanteenTransaction database seeded successfully.");
    }
}
