using CurrencyManagement.Infrastructure.Persistence;
using CurrencyManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyManagement.Infrastructure.Persistence;

/// <summary>
/// Seed data for initial database population
/// </summary>
public class CurrencyDbContextSeed
{
    public static async Task SeedAsync(CurrencyDbContext context)
    {
        var connection = context.Database.GetDbConnection();

        // Seed currencies using raw SQL to properly handle identity columns
        if (!await context.Currencies.AnyAsync())
        {
            await context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO DEAL_CURRMAST (CURR_ID, CURR_NAME, CURR_SYMBOL, CURR_MODIFIEDBY, CURR_MODIFIEDON) 
                VALUES 
                    (1, 'US Dollar', '$', 1, GETUTCDATE()),
                    (2, 'Euro', '€', 1, GETUTCDATE()),
                    (3, 'British Pound', '£', 1, GETUTCDATE()),
                    (4, 'Indian Rupee', '₹', 1, GETUTCDATE()),
                    (5, 'Japanese Yen', '¥', 1, GETUTCDATE()),
                    (6, 'Canadian Dollar', 'C$', 1, GETUTCDATE());
            ");
        }

        // Seed exchange rates using raw SQL
        if (!await context.ExchangeRates.AnyAsync())
        {
            await context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO DEAL_CURRATES (CURRATE_ID, CURRATE_FINYEAR, CURRATE_MONTH, CURRATE_FROMCUR, CURRATE_TOCUR, CURRATE_RATE, CURRATE_MODIFIEDBY, CURRATE_MODIFIEDON)
                VALUES
                    (1, 2026, 1, 2, 1, 1.175, 1, GETUTCDATE()),
                    (2, 2026, 2, 2, 1, 1.180, 1, GETUTCDATE()),
                    (3, 2026, 3, 2, 1, 1.185, 1, GETUTCDATE()),
                    (4, 2026, 3, 3, 1, 1.260, 1, GETUTCDATE()),
                    (5, 2026, 3, 4, 1, 0.0120, 1, GETUTCDATE());
            ");
        }
        else
        {
            // Fix previously truncated rates (DECIMAL(19,0) → DECIMAL(19,6) migration)
            await context.Database.ExecuteSqlRawAsync(@"
                UPDATE DEAL_CURRATES SET CURRATE_RATE = 1.175000 WHERE CURRATE_ID = 1 AND CURRATE_RATE = 1;
                UPDATE DEAL_CURRATES SET CURRATE_RATE = 1.180000 WHERE CURRATE_ID = 2 AND CURRATE_RATE = 1;
                UPDATE DEAL_CURRATES SET CURRATE_RATE = 1.185000 WHERE CURRATE_ID = 3 AND CURRATE_RATE = 1;
                UPDATE DEAL_CURRATES SET CURRATE_RATE = 1.260000 WHERE CURRATE_ID = 4 AND CURRATE_RATE = 1;
                UPDATE DEAL_CURRATES SET CURRATE_RATE = 0.012000 WHERE CURRATE_ID = 5 AND CURRATE_RATE = 0;
            ");
        }

        // Seed organization currency mappings using raw SQL
        if (!await context.OrganizationCurrencyMappings.AnyAsync())
        {
            await context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO DEAL_ORGCURRMAP (ORG_ID, ORG_CURRID, ORG_MODIFIEDBY, ORG_MODIFIEDON)
                VALUES
                    (100, 1, 1, GETUTCDATE()),
                    (100, 2, 1, GETUTCDATE()),
                    (101, 4, 1, GETUTCDATE()),
                    (102, 1, 1, GETUTCDATE()),
                    (102, 3, 1, GETUTCDATE());
            ");
        }
    }
}
