using LookupService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LookupService.Infrastructure.Persistence.Seed;

public static class LookupDbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LookupDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<LookupDbContext>>();

        try
        {
            await db.Database.MigrateAsync();

            if (!await db.LovTypeMasters.AnyAsync())
            {
                await db.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO LOV_TYPEMASTER (LOV_TYPECODE, LOV_TYPENAME) VALUES ('CAT', 'Category');
                    INSERT INTO LOV_TYPEMASTER (LOV_TYPECODE, LOV_TYPENAME) VALUES ('STA', 'Status');
                    INSERT INTO LOV_TYPEMASTER (LOV_TYPECODE, LOV_TYPENAME) VALUES ('PRI', 'Priority');
                    INSERT INTO LOV_TYPEMASTER (LOV_TYPECODE, LOV_TYPENAME) VALUES ('DEP', 'Department');
                    INSERT INTO LOV_TYPEMASTER (LOV_TYPECODE, LOV_TYPENAME) VALUES ('LOC', 'Location');
                ");
            }

            if (!await db.LovMasters.AnyAsync())
            {
                await db.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO LOV_MASTER (LOV_TYPE, LOV_ID, LOV_NAME) VALUES ('CAT', 1, 'General');
                    INSERT INTO LOV_MASTER (LOV_TYPE, LOV_ID, LOV_NAME) VALUES ('CAT', 2, 'Maintenance');
                    INSERT INTO LOV_MASTER (LOV_TYPE, LOV_ID, LOV_NAME) VALUES ('STA', 3, 'Open');
                    INSERT INTO LOV_MASTER (LOV_TYPE, LOV_ID, LOV_NAME) VALUES ('STA', 4, 'Closed');
                    INSERT INTO LOV_MASTER (LOV_TYPE, LOV_ID, LOV_NAME) VALUES ('PRI', 5, 'High');
                    INSERT INTO LOV_MASTER (LOV_TYPE, LOV_ID, LOV_NAME) VALUES ('PRI', 6, 'Medium');
                    INSERT INTO LOV_MASTER (LOV_TYPE, LOV_ID, LOV_NAME) VALUES ('PRI', 7, 'Low');
                ");
            }

            if (!await db.PanelMasters.AnyAsync())
            {
                await db.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO PANEL_MAST (PANEL_ID, PANEL_NAME) VALUES (1, 'Admin Panel');
                    INSERT INTO PANEL_MAST (PANEL_ID, PANEL_NAME) VALUES (2, 'User Panel');
                    INSERT INTO PANEL_MAST (PANEL_ID, PANEL_NAME) VALUES (3, 'Reports Panel');
                ");
            }

            if (!await db.ProcessMasters.AnyAsync())
            {
                await db.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO PROCESS_MASTER (PROCESS_ID, PROCESS_NAME, PROCESS_LIVFLAG) VALUES (1, 'Procurement', 'Y');
                    INSERT INTO PROCESS_MASTER (PROCESS_ID, PROCESS_NAME, PROCESS_LIVFLAG) VALUES (2, 'Inspection', 'Y');
                    INSERT INTO PROCESS_MASTER (PROCESS_ID, PROCESS_NAME, PROCESS_LIVFLAG) VALUES (3, 'Dispatch', 'Y');
                ");
            }

            logger.LogInformation("Database seeded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database");
        }
    }
}
