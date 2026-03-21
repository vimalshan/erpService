using EnergyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EnergyService.Infrastructure.Persistence.Seed;

public static class EnergyDbContextSeed
{
    public static async Task SeedAsync(EnergyDbContext context, ILogger logger)
    {
        if (!await context.EcProcesses.AnyAsync())
        {
            var processes = new List<EcProcess>
            {
                new()
                {
                    EcProcessId = 1,
                    EcProcessDesc = "Electricity Consumption",
                    EcUnitCode = "KWH",
                    EcCloseFlag = "N",
                    LastModifiedBy = 1,
                    LastModifiedOn = DateTime.UtcNow
                },
                new()
                {
                    EcProcessId = 2,
                    EcProcessDesc = "Water Consumption",
                    EcUnitCode = "KL",
                    EcCloseFlag = "N",
                    LastModifiedBy = 1,
                    LastModifiedOn = DateTime.UtcNow
                },
                new()
                {
                    EcProcessId = 3,
                    EcProcessDesc = "Gas Consumption",
                    EcUnitCode = "M3",
                    EcCloseFlag = "N",
                    LastModifiedBy = 1,
                    LastModifiedOn = DateTime.UtcNow
                }
            };

            await context.EcProcesses.AddRangeAsync(processes);

            var readings = new List<EcReading>
            {
                new()
                {
                    EbUnitCode = "KWH",
                    EbProcessId = 1,
                    EbDate = DateTime.UtcNow.AddDays(-2),
                    EbTarget = 1000,
                    EbReading = 500,
                    EbActualUsage = 500,
                    LastModifiedBy = 1,
                    LastModifiedOn = DateTime.UtcNow.AddDays(-2)
                },
                new()
                {
                    EbUnitCode = "KWH",
                    EbProcessId = 1,
                    EbDate = DateTime.UtcNow.AddDays(-1),
                    EbTarget = 1000,
                    EbReading = 1100,
                    EbActualUsage = 600,
                    LastModifiedBy = 1,
                    LastModifiedOn = DateTime.UtcNow.AddDays(-1)
                }
            };

            await context.EcReadings.AddRangeAsync(readings);

            var accesses = new List<EcProcessAccess>
            {
                new()
                {
                    PaProcessId = 1,
                    PaEmpSysId = 101,
                    PaStartDate = DateTime.UtcNow.AddMonths(-6),
                    PaLastModifiedBy = 1,
                    PaLastModifiedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")
                }
            };

            await context.EcProcessAccesses.AddRangeAsync(accesses);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded Energy database with initial data");
        }
    }
}
