using EximManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EximManagement.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(EximDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (!await db.EximProducts.AnyAsync())
        {
            var products = new[]
            {
                Domain.Entities.EximProduct.Create(1001, "Steel Rods", "ORR-001", 1),
                Domain.Entities.EximProduct.Create(1002, "Wheat Grain", "ORR-002", 1),
                Domain.Entities.EximProduct.Create(1003, "Cotton Yarn", "ORR-003", 1),
                Domain.Entities.EximProduct.Create(1004, "Pharmaceutical Tablets", "ORR-004", 1),
                Domain.Entities.EximProduct.Create(1005, "Electronic Components", "ORR-005", 1)
            };

            foreach (var p in products)
            {
                p.ClearDomainEvents();
                await db.EximProducts.AddAsync(p);
            }
            await db.SaveChangesAsync();
        }

        if (!await db.EximProductGroups.AnyAsync())
        {
            var groups = new[]
            {
                Domain.Entities.EximProductGroup.Create(101, "Metals & Alloys", 1),
                Domain.Entities.EximProductGroup.Create(102, "Agricultural Products", 1),
                Domain.Entities.EximProductGroup.Create(103, "Textiles", 1),
                Domain.Entities.EximProductGroup.Create(104, "Pharmaceuticals", 1),
                Domain.Entities.EximProductGroup.Create(105, "Electronics", 1)
            };

            foreach (var g in groups)
            {
                g.ClearDomainEvents();
                await db.EximProductGroups.AddAsync(g);
            }
            await db.SaveChangesAsync();
        }

        if (!await db.EximUserMasters.AnyAsync())
        {
            var users = new[]
            {
                Domain.Entities.EximUserMaster.Create(0, 1001, "SPARSH001", DateTime.UtcNow.AddYears(-2), 1),
                Domain.Entities.EximUserMaster.Create(0, 1002, "SPARSH002", DateTime.UtcNow.AddYears(-1), 1)
            };

            foreach (var u in users) await db.EximUserMasters.AddAsync(u);
            await db.SaveChangesAsync();
        }
    }
}
