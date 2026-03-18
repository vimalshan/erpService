using GroupManagementService.Domain.Entities;
using GroupManagementService.Domain.ValueObjects;
using GroupManagementService.Infrastructure.Persistence;

namespace GroupManagementService.Infrastructure.Seeds
{
    public static class GroupManagementSeeds
    {
        public static async Task SeedDataAsync(GroupManagementDbContext context)
        {
            try
            {
                if (!context.Groups.Any())
                {
                    var adminUser = 1L; // System admin user ID

                    // Create admin group
                    var adminGroup = new Group(
                        "ADMIN",
                        "Administrator Group",
                        "Group with full system access",
                        adminUser,
                        isAdmin: true);

                    // Create user group
                    var userGroup = new Group(
                        "USER",
                        "User Group",
                        "Standard user group with limited access",
                        adminUser,
                        isAdmin: false);

                    // Create manager group
                    var managerGroup = new Group(
                        "MANAGER",
                        "Manager Group",
                        "Manager group with approval permissions",
                        adminUser,
                        isAdmin: false);

                    // Add groups to context
                    context.Groups.AddRange(adminGroup, userGroup, managerGroup);

                    // Refresh context to get generated IDs
                    await context.SaveChangesAsync();

                    // Add menu mappings for admin group
                    var adminMenuMaps = new List<GroupMenuMap>
                    {
                        new GroupMenuMap(
                            adminGroup.Id,
                            "GROUP_MNGMT",
                            "Group Management",
                            new MenuPermissions(true, true, true, true, true),
                            adminUser,
                            1),
                        new GroupMenuMap(
                            adminGroup.Id,
                            "USER_MNGMT",
                            "User Management",
                            new MenuPermissions(true, true, true, true, true),
                            adminUser,
                            2),
                        new GroupMenuMap(
                            adminGroup.Id,
                            "AUDIT_LOG",
                            "Audit Log",
                            new MenuPermissions(true, false, false, false, false),
                            adminUser,
                            3)
                    };

                    // Add menu mappings for user group
                    var userMenuMaps = new List<GroupMenuMap>
                    {
                        new GroupMenuMap(
                            userGroup.Id,
                            "GROUP_MNGMT",
                            "Group Management",
                            new MenuPermissions(true, false, false, false, false),
                            adminUser,
                            1),
                        new GroupMenuMap(
                            userGroup.Id,
                            "USER_MNGMT",
                            "User Management",
                            MenuPermissions.ViewOnly,
                            adminUser,
                            2)
                    };

                    // Add menu mappings for manager group
                    var managerMenuMaps = new List<GroupMenuMap>
                    {
                        new GroupMenuMap(
                            managerGroup.Id,
                            "GROUP_MNGMT",
                            "Group Management",
                            new MenuPermissions(true, true, true, false, true),
                            adminUser,
                            1),
                        new GroupMenuMap(
                            managerGroup.Id,
                            "USER_MNGMT",
                            "User Management",
                            new MenuPermissions(true, true, true, false, false),
                            adminUser,
                            2),
                        new GroupMenuMap(
                            managerGroup.Id,
                            "AUDIT_LOG",
                            "Audit Log",
                            MenuPermissions.ViewOnly,
                            adminUser,
                            3)
                    };

                    context.GroupMenuMaps.AddRange(adminMenuMaps);
                    context.GroupMenuMaps.AddRange(userMenuMaps);
                    context.GroupMenuMaps.AddRange(managerMenuMaps);

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error seeding GroupManagement database", ex);
            }
        }
    }
}
