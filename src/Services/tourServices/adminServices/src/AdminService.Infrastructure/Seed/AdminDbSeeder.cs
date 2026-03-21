using AdminService.Domain.Entities;
using AdminService.Infrastructure.Data;

namespace AdminService.Infrastructure.Seed;

public static class AdminDbSeeder
{
    public static async Task SeedAsync(AdminDbContext context)
    {
        if (!context.AdminMasters.Any())
        {
            var masters = new List<AdminMaster>
            {
                new()
                {
                    AdminId = "ADM001",
                    AdminName = "Head Office Admin",
                    AdminPic = "headoffice.jpg",
                    AdminUnitId = "UNIT001",
                    AdminUnitHeadSysId = "EMP001",
                    AdminLocStatus = 'A'
                },
                new()
                {
                    AdminId = "ADM002",
                    AdminName = "Branch Office Admin",
                    AdminPic = "branchoffice.jpg",
                    AdminUnitId = "UNIT002",
                    AdminUnitHeadSysId = "EMP002",
                    AdminLocStatus = 'A'
                }
            };
            await context.AdminMasters.AddRangeAsync(masters);
        }

        if (!context.AdminUserMaps.Any())
        {
            var userMaps = new List<AdminUserMap>
            {
                new()
                {
                    AdminMapId = "MAP001",
                    AdminBookType = "TKT",
                    AdminMode = "Flight",
                    AdminEmpSysId = "EMP100",
                    AdminId = "ADM001",
                    AdminLastModifiedBy = "SYSTEM",
                    AdminLastModifiedOn = DateTime.UtcNow
                },
                new()
                {
                    AdminMapId = "MAP002",
                    AdminBookType = "CAB",
                    AdminMode = "Sedan",
                    AdminEmpSysId = "EMP101",
                    AdminId = "ADM001",
                    AdminLastModifiedBy = "SYSTEM",
                    AdminLastModifiedOn = DateTime.UtcNow
                }
            };
            await context.AdminUserMaps.AddRangeAsync(userMaps);
        }

        if (!context.AdminFinUserMaps.Any())
        {
            var finMaps = new List<AdminFinUserMap>
            {
                new()
                {
                    FinanceMapId = "FIN001",
                    FinancePayUnitId = "PAY001",
                    FinanceEmpSysId = "EMP200",
                    FinanceLastModifiedBy = "SYSTEM",
                    FinanceLastModifiedOn = DateTime.UtcNow
                }
            };
            await context.AdminFinUserMaps.AddRangeAsync(finMaps);
        }

        if (!context.AdminAccessRights.Any())
        {
            var rights = new List<AdminAccessRights>
            {
                new()
                {
                    AdminRightsId = "RIGHT001",
                    AdminLocationId = "ADM001",
                    AdminRightsFor = "Admin",
                    AdminRightsType = "TKT",
                    AdminUserId = "USR001",
                    AdminAlertId = "ALT001",
                    AdminContactNo = "1234567890",
                    AdminContactDes = "Primary Contact",
                    AdminEntOn = DateTime.UtcNow,
                    AdminEntBy = "SYSTEM"
                }
            };
            await context.AdminAccessRights.AddRangeAsync(rights);
        }

        await context.SaveChangesAsync();
    }
}
