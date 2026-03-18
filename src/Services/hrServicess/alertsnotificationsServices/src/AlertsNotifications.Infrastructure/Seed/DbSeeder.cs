using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlertsNotifications.Infrastructure.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AlertsNotificationsDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AlertsNotificationsDbContext>>();

        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migration completed.");

            if (!await context.AlertMasters.AnyAsync())
            {
                context.AlertMasters.AddRange(
                    new AlertMaster
                    {
                        AlertId = 1, AlertApps = "HR", AlertName = "Leave Approval",
                        AlertType = "WD", AlertDesc = "Alert for leave approval workflow",
                        AlertToDesc = "Direct Manager", AlertGradeCat = "ALL"
                    },
                    new AlertMaster
                    {
                        AlertId = 2, AlertApps = "HR", AlertName = "Probation Reminder",
                        AlertType = "SD", AlertDesc = "Scheduled alert for probation confirmation",
                        AlertToDesc = "HR Admin", AlertGradeCat = "ALL"
                    },
                    new AlertMaster
                    {
                        AlertId = 3, AlertApps = "HR", AlertName = "Circular Notification",
                        AlertType = "WO", AlertDesc = "Workflow alert for new circular distribution",
                        AlertToDesc = "All Employees", AlertGradeCat = "ALL"
                    }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded AlertMaster data.");
            }

            if (!await context.AlertGroups.AnyAsync())
            {
                context.AlertGroups.AddRange(
                    new AlertGroup
                    {
                        AlertGroupId = 1, AlertGroupName = "HR Managers",
                        AlertGroupType = 'R', CreatedBy = 1, CreatedOn = DateTime.UtcNow
                    },
                    new AlertGroup
                    {
                        AlertGroupId = 2, AlertGroupName = "Payroll Team",
                        AlertGroupType = 'P', CreatedBy = 1, CreatedOn = DateTime.UtcNow
                    }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded AlertGroup data.");
            }

            if (!await context.CircularTemplates.AnyAsync())
            {
                context.CircularTemplates.AddRange(
                    new CircularTemplate
                    {
                        CircularTemplateId = 1, CircularTemplateApplyToUnit = 1,
                        CircularTemplateUnitId = 1, CircularTemplateTypeId = 1,
                        CircularTemplateName = "General Circular Template",
                        CircularTemplateHtml = "<h1>Circular</h1><p>{{content}}</p>",
                        CircularTemplateModifiedBy = 1, CircularTemplateModifiedOn = DateTime.UtcNow
                    }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded CircularTemplate data.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
