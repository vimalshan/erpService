using Microsoft.Extensions.Logging;
using travelTransactionService.Domain.Entities;

namespace travelTransactionService.Infrastructure.Data.Seed;

public static class TransactionDbSeeder
{
    public static async Task SeedAsync(TransactionDbContext context, ILogger logger)
    {
        if (!context.VendorMasters.Any())
        {
            var vendors = new[]
            {
                VendorMaster.Create(1001, "ABC Travel Agency", "V", "123 Main Street", "9876543210", "ABCDE1234F"),
                VendorMaster.Create(1002, "XYZ Hotels", "H", "456 Park Avenue", "9876543211", "XYZAB5678G"),
                VendorMaster.Create(1003, "PQR Cab Services", "V", "789 Ring Road", "9876543212", "PQRST9012H")
            };
            context.VendorMasters.AddRange(vendors);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} vendors", vendors.Length);
        }

        if (!context.TaxMasters.Any())
        {
            var taxes = new[]
            {
                TaxMaster.Create(1001, "SGST ", 9, DateTime.Parse("2024-01-01")),
                TaxMaster.Create(1001, "CGST ", 9, DateTime.Parse("2024-01-01")),
                TaxMaster.Create(1001, "IGST ", 18, DateTime.Parse("2024-01-01"))
            };
            context.TaxMasters.AddRange(taxes);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} tax masters", taxes.Length);
        }

        if (!context.TravelApParams.Any())
        {
            context.TravelApParams.Add(TravelApParams.Create(1, "O", "TRVL001", 100001));
            context.TravelApParams.Add(TravelApParams.Create(2, "P", "TRVL002", 100002));
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded travel AP params");
        }

        logger.LogInformation("Transaction database seeding completed");
    }
}
