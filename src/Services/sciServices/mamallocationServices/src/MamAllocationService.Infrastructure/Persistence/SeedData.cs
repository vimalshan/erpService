using MamAllocationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MamAllocationService.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(MamAllocationDbContext context)
    {
        if (await context.AllocationDetails.AnyAsync()) return;

        var allocationDetails = new[]
        {
            new AllocationDetail
            {
                AllDate = new DateTime(2026, 1, 1),
                AllRm = 1001,
                AllEntOpen = 5000,
                AllProd = 2000,
                AllCons = 1500,
                AllSale = 800,
                AllNetEnt = 3500
            },
            new AllocationDetail
            {
                AllDate = new DateTime(2026, 1, 1),
                AllRm = 1002,
                AllEntOpen = 3000,
                AllProd = 1000,
                AllCons = 800,
                AllSale = 500,
                AllNetEnt = 2200
            },
            new AllocationDetail
            {
                AllDate = new DateTime(2026, 2, 1),
                AllRm = 1001,
                AllEntOpen = 4500,
                AllProd = 1800,
                AllCons = 1200,
                AllSale = 600,
                AllNetEnt = 3300
            }
        };

        await context.AllocationDetails.AddRangeAsync(allocationDetails);
        await context.SaveChangesAsync();

        // Keyless tables must be seeded via raw SQL since EF cannot track them
        await context.Database.ExecuteSqlRawAsync(
            """
            INSERT INTO MAM_ARRIVAL_DET (ARRIVAL_NO, ARRIVAL_DATE, ARRIVAL_QTY, ARRIVAL_ITEM, ARRIVAL_RECEIPTNO)
            VALUES (1, '2026-01-05', 1000, 1001, 100001),
                   (2, '2026-01-10', 2000, 1002, 100002)
            """);

        await context.Database.ExecuteSqlRawAsync(
            """
            INSERT INTO MAM_CONSUMPTION_DET (CONSUMPTION_NO, CONSUMPTION_DATE, CONSUMPTION_RM, CONSUMPTION_QTY)
            VALUES (1, '2026-01-06', 1001, 500),
                   (2, '2026-01-12', 1002, 300)
            """);

        await context.Database.ExecuteSqlRawAsync(
            """
            INSERT INTO MAM_DISPATCH_DET (DISPATCH_NO, DISPATCH_DATE, DISPATCH_FG, DISPATCH_QTY, DISPATCH_TYPE, DISPATCH_INVOICENO)
            VALUES (1, '2026-01-07', 2001, 100, 'D', 'INV-001'),
                   (2, '2026-01-14', 2002, 200, 'E', 'INV-002')
            """);
    }
}
