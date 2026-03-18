using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VendorService.Domain.Entities;

namespace VendorService.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(VendorDbContext context, ILogger logger)
    {
        if (await context.VendorMasters.AnyAsync())
        {
            logger.LogInformation("Seed data already present — skipping.");
            return;
        }

        logger.LogInformation("Seeding vendor data...");

        var vendors = new[]
        {
            VendorMaster.Create(1, 10, 1, "Acme Supplies Pvt Ltd",  "contact@acme.in",    "12 MG Road, Bengaluru",   1),
            VendorMaster.Create(2, 10, 1, "Phoenix Traders",         "info@phoenix.in",    "45 Gandhi Nagar, Pune",   1),
            VendorMaster.Create(3, 11, 2, "SteelWorks India",        "sales@steel.in",     "8 Industrial Area, Surat",1),
            VendorMaster.Create(4, 12, 3, "GlobalParts Co",          "gp@globalparts.in",  "77 Anna Salai, Chennai",  2),
            VendorMaster.Create(5, 10, 1, "SwiftLogistics",          null,                 "99 NH-48, Delhi",         1),
        };

        context.VendorMasters.AddRange(vendors);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} vendor records.", vendors.Length);

        if (!await context.TdsFileDetails.AnyAsync())
        {
            var tdsFiles = new[]
            {
                TdsFileDetail.Create(1, "Q1_TDS_2025.zip", "AAAPA1234A", "S", "ZIP"),
                TdsFileDetail.Create(2, "Q2_TDS_2025.zip", "BBBPB5678B", "P", "ZIP"),
                TdsFileDetail.Create(3, "Q3_TDS_2025.pdf", "CCCPC9012C", "F", "PDF"),
            };

            context.TdsFileDetails.AddRange(tdsFiles);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} TDS file detail records.", tdsFiles.Length);
        }
    }
}
