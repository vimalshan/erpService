using Microsoft.EntityFrameworkCore;
using ProblemManagement.Domain.Entities;

namespace ProblemManagement.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(ProblemManagementDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.ProblemFunctions.AnyAsync())
        {
            context.ProblemFunctions.AddRange(
                new ProblemFunction { FuncId = 1, FuncName = "IT Infrastructure" },
                new ProblemFunction { FuncId = 2, FuncName = "Software Development" },
                new ProblemFunction { FuncId = 3, FuncName = "Network Operations" },
                new ProblemFunction { FuncId = 4, FuncName = "Security" },
                new ProblemFunction { FuncId = 5, FuncName = "Database Administration" },
                new ProblemFunction { FuncId = 6, FuncName = "User Support" },
                new ProblemFunction { FuncId = 7, FuncName = "Hardware" },
                new ProblemFunction { FuncId = 8, FuncName = "Cloud Services" }
            );
        }

        if (!await context.ProblemImpacts.AnyAsync())
        {
            context.ProblemImpacts.AddRange(
                new ProblemImpact { ImpactId = 1, ImpactDesc = "Critical - Complete service outage" },
                new ProblemImpact { ImpactId = 2, ImpactDesc = "High - Major functionality affected" },
                new ProblemImpact { ImpactId = 3, ImpactDesc = "Medium - Partial functionality affected" },
                new ProblemImpact { ImpactId = 4, ImpactDesc = "Low - Minor inconvenience" },
                new ProblemImpact { ImpactId = 5, ImpactDesc = "Informational - No immediate impact" }
            );
        }

        if (!await context.Problems.AnyAsync())
        {
            var problem = new ProblemMain
            {
                PrId = 1,
                PrOwner = 1001,
                PrEnteredBy = 1001,
                PrDescription = "Slow API response times affecting production",
                PrCategory = "1",
                PrImpact = "Users experiencing 5+ second load times",
                PrExpResult = "API response < 200ms",
                PrEnteredOn = DateTime.UtcNow,
                PrStatus = "P",
                PrUnitId = 10,
                PrSiteId = 1,
                PrModBy = 1001,
                PrModOn = DateTime.UtcNow
            };
            context.Problems.Add(problem);

            context.ProblemSolutions.Add(new ProblemSolution
            {
                SolId = 1,
                SolPrId = 1,
                SolDescription = "Implement database query optimization and caching",
                SolEnteredBy = 1002,
                SolEnteredOn = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
    }
}
