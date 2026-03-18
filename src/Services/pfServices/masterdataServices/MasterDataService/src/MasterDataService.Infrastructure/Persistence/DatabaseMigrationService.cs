using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Infrastructure.Persistence;

public static class DatabaseMigrationService
{
    public static async Task MigrateDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MasterDataDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<MasterDataDbContext>>();
        try
        {
            logger.LogInformation("Starting database migration...");
            await context.Database.MigrateAsync();
            await SeedDataAsync(context, logger);
            logger.LogInformation("Database migration completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database migration failed.");
            throw;
        }
    }

    private static async Task SeedDataAsync(MasterDataDbContext context, ILogger logger)
    {
        if (!await context.FundTypes.AnyAsync())
        {
            await context.FundTypes.AddRangeAsync(
                new Domain.Entities.FundType { FundTypeCode = "EPF", FundTypeName = "Employee Provident Fund" },
                new Domain.Entities.FundType { FundTypeCode = "VPF", FundTypeName = "Voluntary Provident Fund" },
                new Domain.Entities.FundType { FundTypeCode = "GPF", FundTypeName = "General Provident Fund" });
            logger.LogInformation("Seeded FundType data.");
        }

        if (!await context.LovMasters.AnyAsync())
        {
            await context.LovMasters.AddRangeAsync(
                new Domain.Entities.LovMaster { LovId = 1, LovCode = "A", LovDescription = "Active", LovValue = "A", LovCategory = "STATUS", LovStatus = "A" },
                new Domain.Entities.LovMaster { LovId = 2, LovCode = "I", LovDescription = "Inactive", LovValue = "I", LovCategory = "STATUS", LovStatus = "A" },
                new Domain.Entities.LovMaster { LovId = 3, LovCode = "Y", LovDescription = "Yes", LovValue = "Y", LovCategory = "YESNO", LovStatus = "A" },
                new Domain.Entities.LovMaster { LovId = 4, LovCode = "N", LovDescription = "No", LovValue = "N", LovCategory = "YESNO", LovStatus = "A" },
                new Domain.Entities.LovMaster { LovId = 5, LovCode = "MO", LovDescription = "Monthly", LovValue = "MO", LovCategory = "FREQUENCY", LovStatus = "A" },
                new Domain.Entities.LovMaster { LovId = 6, LovCode = "QT", LovDescription = "Quarterly", LovValue = "QT", LovCategory = "FREQUENCY", LovStatus = "A" },
                new Domain.Entities.LovMaster { LovId = 7, LovCode = "AN", LovDescription = "Annually", LovValue = "AN", LovCategory = "FREQUENCY", LovStatus = "A" });
            logger.LogInformation("Seeded LOV Master data.");
        }

        if (!await context.StatusMasters.AnyAsync())
        {
            await context.StatusMasters.AddRangeAsync(
                new Domain.Entities.StatusMaster { StatusType = "ME", StatusCodeValue = "AC", StatusName = "Active Member" },
                new Domain.Entities.StatusMaster { StatusType = "ME", StatusCodeValue = "IN", StatusName = "Inactive Member" },
                new Domain.Entities.StatusMaster { StatusType = "ME", StatusCodeValue = "LF", StatusName = "Left Fund" },
                new Domain.Entities.StatusMaster { StatusType = "CL", StatusCodeValue = "OP", StatusName = "Open" },
                new Domain.Entities.StatusMaster { StatusType = "CL", StatusCodeValue = "CL", StatusName = "Closed" });
            logger.LogInformation("Seeded Status Master data.");
        }

        if (!await context.RateTypes.AnyAsync())
        {
            await context.RateTypes.AddRangeAsync(
                new Domain.Entities.RateType { RateTypeCode = "INT", RateTypeName = "Interest Rate" },
                new Domain.Entities.RateType { RateTypeCode = "CON", RateTypeName = "Contribution Rate" },
                new Domain.Entities.RateType { RateTypeCode = "ADM", RateTypeName = "Admin Charges" });
            logger.LogInformation("Seeded Rate Type data.");
        }

        if (!await context.ComputationMonths.AnyAsync())
        {
            await context.ComputationMonths.AddRangeAsync([
                new Domain.Entities.ComputationMonth { SerialNumber = 1, MonthName = "April" },
                new Domain.Entities.ComputationMonth { SerialNumber = 2, MonthName = "May" },
                new Domain.Entities.ComputationMonth { SerialNumber = 3, MonthName = "June" },
                new Domain.Entities.ComputationMonth { SerialNumber = 4, MonthName = "July" },
                new Domain.Entities.ComputationMonth { SerialNumber = 5, MonthName = "August" },
                new Domain.Entities.ComputationMonth { SerialNumber = 6, MonthName = "September" },
                new Domain.Entities.ComputationMonth { SerialNumber = 7, MonthName = "October" },
                new Domain.Entities.ComputationMonth { SerialNumber = 8, MonthName = "November" },
                new Domain.Entities.ComputationMonth { SerialNumber = 9, MonthName = "December" },
                new Domain.Entities.ComputationMonth { SerialNumber = 10, MonthName = "January" },
                new Domain.Entities.ComputationMonth { SerialNumber = 11, MonthName = "February" },
                new Domain.Entities.ComputationMonth { SerialNumber = 12, MonthName = "March" }]);
            logger.LogInformation("Seeded Computation Month data.");
        }

        if (!await context.Configurations.AnyAsync())
        {
            await context.Configurations.AddRangeAsync(
                new Domain.Entities.Configuration { ConfigKey = "PF_RATE_DEFAULT", ConfigValue = "12", ConfigType = "DECIMAL", ConfigDescription = "Default PF contribution rate", CreatedDate = DateTime.UtcNow, CreatedBy = 1 },
                new Domain.Entities.Configuration { ConfigKey = "INTEREST_RATE_CURRENT", ConfigValue = "8.15", ConfigType = "DECIMAL", ConfigDescription = "Current EPF interest rate", CreatedDate = DateTime.UtcNow, CreatedBy = 1 },
                new Domain.Entities.Configuration { ConfigKey = "ADMIN_CHARGES_RATE", ConfigValue = "0.01", ConfigType = "DECIMAL", ConfigDescription = "Admin charges on EPF", CreatedDate = DateTime.UtcNow, CreatedBy = 1 });
            logger.LogInformation("Seeded Configuration data.");
        }

        if (!await context.InvestmentCategoryGroups.AnyAsync())
        {
            await context.InvestmentCategoryGroups.AddRangeAsync(
                new Domain.Entities.InvestmentCategoryGroup { GroupId = 1, ShortName = "GOVT", GroupName = "Government Securities" },
                new Domain.Entities.InvestmentCategoryGroup { GroupId = 2, ShortName = "BOND", GroupName = "Bonds and Debentures" },
                new Domain.Entities.InvestmentCategoryGroup { GroupId = 3, ShortName = "EQTY", GroupName = "Equity" });
            logger.LogInformation("Seeded Investment Category Group data.");
        }

        if (!await context.RoleMasters.AnyAsync())
        {
            await context.RoleMasters.AddRangeAsync(
                new Domain.Entities.RoleMaster { RoleCode = 1, RoleName = "Administrator", RoleDescription = "Full system access", RoleStatus = "A" },
                new Domain.Entities.RoleMaster { RoleCode = 2, RoleName = "Fund Manager", RoleDescription = "Manages fund operations", RoleStatus = "A" },
                new Domain.Entities.RoleMaster { RoleCode = 3, RoleName = "Accounts Officer", RoleDescription = "Accounts and reconciliation", RoleStatus = "A" },
                new Domain.Entities.RoleMaster { RoleCode = 4, RoleName = "Viewer", RoleDescription = "Read-only access", RoleStatus = "A" });
            logger.LogInformation("Seeded Role Master data.");
        }

        await context.SaveChangesAsync();
    }
}
