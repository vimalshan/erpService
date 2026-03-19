using MenuAndSecurityService.Domain.Entities;
using MenuAndSecurityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MenuAndSecurityService.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MenuSecurityDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<MenuSecurityDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.MenuMasters.AnyAsync())
            {
                var now = DateTime.UtcNow;

                var menus = new List<MenuMaster>
                {
                    new() { MenuId = 1, MenuName = "Dashboard", MenuPageName = "/dashboard", MenuParentId = null, MenuDisplayOrder = 1, ModifiedBy = 1, ModifiedOn = now },
                    new() { MenuId = 2, MenuName = "Administration", MenuPageName = "/admin", MenuParentId = null, MenuDisplayOrder = 2, ModifiedBy = 1, ModifiedOn = now },
                    new() { MenuId = 6, MenuName = "Reports", MenuPageName = "/reports", MenuParentId = null, MenuDisplayOrder = 3, ModifiedBy = 1, ModifiedOn = now },
                    new() { MenuId = 9, MenuName = "Settings", MenuPageName = "/settings", MenuParentId = null, MenuDisplayOrder = 4, ModifiedBy = 1, ModifiedOn = now }
                };

                context.MenuMasters.AddRange(menus);
                await context.SaveChangesAsync();

                var childMenus = new List<MenuMaster>
                {
                    new() { MenuId = 3, MenuName = "User Management", MenuPageName = "/admin/users", MenuParentId = 2, MenuDisplayOrder = 1, ModifiedBy = 1, ModifiedOn = now },
                    new() { MenuId = 4, MenuName = "Role Management", MenuPageName = "/admin/roles", MenuParentId = 2, MenuDisplayOrder = 2, ModifiedBy = 1, ModifiedOn = now },
                    new() { MenuId = 5, MenuName = "Menu Configuration", MenuPageName = "/admin/menus", MenuParentId = 2, MenuDisplayOrder = 3, ModifiedBy = 1, ModifiedOn = now },
                    new() { MenuId = 7, MenuName = "Security Audit", MenuPageName = "/reports/security-audit", MenuParentId = 6, MenuDisplayOrder = 1, ModifiedBy = 1, ModifiedOn = now },
                    new() { MenuId = 8, MenuName = "Access Log", MenuPageName = "/reports/access-log", MenuParentId = 6, MenuDisplayOrder = 2, ModifiedBy = 1, ModifiedOn = now },
                    new() { MenuId = 10, MenuName = "General Settings", MenuPageName = "/settings/general", MenuParentId = 9, MenuDisplayOrder = 1, ModifiedBy = 1, ModifiedOn = now }
                };

                context.MenuMasters.AddRange(childMenus);

                var accesses = new List<RoleMenuAccess>
                {
                    // Admin role (RoleId=1) gets access to all menus
                    new() { MenuAccessId = 1, MenuId = 1, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 2, MenuId = 2, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 3, MenuId = 3, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 4, MenuId = 4, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 5, MenuId = 5, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 6, MenuId = 6, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 7, MenuId = 7, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 8, MenuId = 8, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 9, MenuId = 9, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 10, MenuId = 10, MenuRoleId = 1, RoleModifiedBy = 1, RoleModifiedOn = now },
                    // User role (RoleId=2) gets limited access
                    new() { MenuAccessId = 11, MenuId = 1, MenuRoleId = 2, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 12, MenuId = 6, MenuRoleId = 2, RoleModifiedBy = 1, RoleModifiedOn = now },
                    new() { MenuAccessId = 13, MenuId = 8, MenuRoleId = 2, RoleModifiedBy = 1, RoleModifiedOn = now }
                };

                context.RoleMenuAccesses.AddRange(accesses);

                await context.SaveChangesAsync();
                logger.LogInformation("Database seeded successfully with {MenuCount} menus and {AccessCount} access entries",
                    menus.Count, accesses.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database");
            throw;
        }
    }
}
