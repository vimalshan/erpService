using CategoryAndVendorService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CategoryAndVendorService.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(CategoryVendorDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.MainCategories.AnyAsync()) return;

        var mainCat1 = MainCategory.Create(1, "IT Equipment", 1, 1);
        var mainCat2 = MainCategory.Create(2, "Office Supplies", 2, 1);
        var mainCat3 = MainCategory.Create(3, "Furniture", 3, 1);

        // Clear domain events since we're seeding directly
        mainCat1.ClearDomainEvents();
        mainCat2.ClearDomainEvents();
        mainCat3.ClearDomainEvents();

        context.MainCategories.AddRange(mainCat1, mainCat2, mainCat3);
        await context.SaveChangesAsync();

        var sub1 = SubCategory.Create(1, 1, "Laptops", 1);
        var sub2 = SubCategory.Create(2, 1, "Monitors", 1);
        var sub3 = SubCategory.Create(3, 2, "Paper", 1);
        var sub4 = SubCategory.Create(4, 2, "Pens", 1);
        var sub5 = SubCategory.Create(5, 3, "Desks", 1);
        var sub6 = SubCategory.Create(6, 3, "Chairs", 1);

        sub1.ClearDomainEvents();
        sub2.ClearDomainEvents();
        sub3.ClearDomainEvents();
        sub4.ClearDomainEvents();
        sub5.ClearDomainEvents();
        sub6.ClearDomainEvents();

        context.SubCategories.AddRange(sub1, sub2, sub3, sub4, sub5, sub6);
        await context.SaveChangesAsync();
    }
}
