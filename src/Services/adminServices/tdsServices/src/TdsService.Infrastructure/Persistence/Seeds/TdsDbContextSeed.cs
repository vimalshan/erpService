using Microsoft.EntityFrameworkCore;
using TdsService.Domain.Entities;
using TdsService.Domain.ValueObjects;

namespace TdsService.Infrastructure.Persistence.Seeds;

public static class TdsDbContextSeed
{
    public static async Task SeedAsync(TdsDbContext context)
    {
        if (!await context.TdsVendors.AnyAsync())
        {
            var vendors = new List<TdsVendor>
            {
                TdsVendor.Create(1, "Acme Pvt Ltd",      "acme@example.com",    "ABCDE1234F"),
                TdsVendor.Create(2, "Global Corp",        "global@example.com",  "BCDEF2345G"),
                TdsVendor.Create(3, "Tech Solutions Ltd", "tech@example.com",    "CDEFG3456H"),
            };

            foreach (var v in vendors)
                v.ClearDomainEvents();   // skip events during seeding

            await context.TdsVendors.AddRangeAsync(vendors);
        }

        if (!await context.TdsFiles.AnyAsync())
        {
            var files = new List<TdsFile>
            {
                TdsFile.Create(101, "Form16A_FY2425_Q1.pdf", "ABCDE1234F", "N", "16A"),
                TdsFile.Create(102, "Form26A_FY2425_Q1.pdf", "BCDEF2345G", "Y", "26A"),
            };

            foreach (var f in files)
                f.ClearDomainEvents();   // skip events during seeding

            await context.TdsFiles.AddRangeAsync(files);
        }

        await context.SaveChangesAsync();
    }
}
