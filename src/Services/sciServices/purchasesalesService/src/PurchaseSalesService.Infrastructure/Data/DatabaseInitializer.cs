using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            await db.Database.MigrateAsync();
            logger.LogInformation("Database migrated successfully.");

            await SeedAsync(db, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    private static async Task SeedAsync(ApplicationDbContext db, ILogger logger)
    {
        if (await db.PurchaseDetails.AnyAsync()) return;

        logger.LogInformation("Seeding initial data...");

        var now = DateTime.UtcNow;

        // ── PURCHASE_DETAILS ──────────────────────────────────────────────────
        var purchase = PurchaseDetail.Create(
            trackingNumber: 100001,
            transactionNumber: 1,
            purposeCode: 1,
            stageCode: 1,
            supplierCode: "SUP001",
            userId: "SYSTEM",
            userNumber: 0);

        purchase.ClearDomainEvents();
        db.PurchaseDetails.Add(purchase);

        // ── SALE_MAIN ─────────────────────────────────────────────────────────
        var sale = SaleMain.Create(
            trackingNumber: 200001,
            transactionNumber: 1,
            purposeCode: 1,
            stageCode: 1,
            userId: "SYSTEM",
            userNumber: 0,
            vehicleCustomer: "DEMO-CUST-001");

        sale.ClearDomainEvents();
        db.SaleMains.Add(sale);

        // Save first so that identity PKs are generated (needed for FK seeds below)
        await db.SaveChangesAsync();

        // ── SALE_SUB ──────────────────────────────────────────────────────────
        var saleSub = SaleSub.Create(
            saleSerialNumber: sale.SerialNumber,
            productCode: "PROD-001",
            quantity: 10,
            productGrade: "A",
            userComment: "Seed line item",
            checkbookInvoice: "INV-SEED-001");

        db.SaleSubs.Add(saleSub);

        // ── LOG_PURCHASE_DETAILS ──────────────────────────────────────────────
        db.LogPurchaseDetails.Add(new LogPurchaseDetail
        {
            TrackingNumber    = purchase.TrackingNumber,
            TransactionNumber = purchase.TransactionNumber,
            PurposeCode       = purchase.PurposeCode,
            StageCode         = purchase.StageCode,
            SupplierCode      = purchase.SupplierCode,
            UserId            = purchase.UserId ?? "SYSTEM",
            UserNumber        = purchase.UserNumber ?? 0,
            UpdatedAt         = purchase.UpdatedAt,
            CancelFlag        = purchase.CancelFlag,
            ModifiedBy        = "SYSTEM",
            ModifiedByNumber  = 0,
            ModifiedAt        = now
        });

        // ── LOG_SALE_MAIN ─────────────────────────────────────────────────────
        db.LogSaleMains.Add(new LogSaleMain
        {
            TrackingNumber    = sale.TrackingNumber,
            TransactionNumber = sale.TransactionNumber,
            PurposeCode       = sale.PurposeCode,
            StageCode         = sale.StageCode,
            IsoNumber         = sale.IsoNumber,
            IsoDate           = sale.IsoDate,
            ProductDescription= sale.ProductDescription,
            UserId            = sale.UserId,
            UserNumber        = sale.UserNumber,
            UpdatedAt         = sale.UpdatedAt,
            CancelFlag        = sale.CancelFlag,
            ModifiedBy        = "SYSTEM",
            ModifiedByNumber  = 0,
            ModifiedAt        = now
        });

        // ── LOG_SALE_SUB ──────────────────────────────────────────────────────
        db.LogSaleSubs.Add(new LogSaleSub
        {
            SerialNumber     = sale.SerialNumber,
            ProductCode      = "PROD-001",
            ProductQuantity  = 10,
            ProductGrade     = "A",
            UserComment      = "Seed log line item",
            CancelFlag       = 'N',
            ModifiedBy       = "SYSTEM",
            ModifiedByNumber = 0,
            ModifiedAt       = now
        });

        await db.SaveChangesAsync();
        logger.LogInformation("Seed data inserted.");
    }
}
