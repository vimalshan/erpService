using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceService.Infrastructure.Persistence.Seed;

public static class FinanceDbContextSeed
{
    public static async Task SeedAsync(FinanceDbContext context, ILogger logger)
    {
        if (!await context.PaymentTerms.AnyAsync())
        {
            context.PaymentTerms.AddRange(GetSeedPaymentTerms());
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded Payment Terms data.");
        }

        if (!await context.ApInvoices.AnyAsync())
        {
            context.ApInvoices.AddRange(GetSeedInvoices());
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded AP Invoices data.");
        }

        if (!await context.TravelBatchMains.AnyAsync())
        {
            context.TravelBatchMains.AddRange(GetSeedBatches());
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded Travel Batch data.");
        }
    }

    private static IEnumerable<PaymentTerm> GetSeedPaymentTerms()
    {
        return new List<PaymentTerm>
        {
            new() { TermId = 1, Name = "Net 30", EnabledFlag = "Y", DueCutoffDay = 30, Description = "Payment due in 30 days", CreationDate = DateTime.UtcNow, LastUpdateDate = DateTime.UtcNow, CreatedBy = 1, LastUpdatedBy = 1 },
            new() { TermId = 2, Name = "Net 60", EnabledFlag = "Y", DueCutoffDay = 60, Description = "Payment due in 60 days", CreationDate = DateTime.UtcNow, LastUpdateDate = DateTime.UtcNow, CreatedBy = 1, LastUpdatedBy = 1 },
            new() { TermId = 3, Name = "Immediate", EnabledFlag = "Y", DueCutoffDay = 0, Description = "Immediate payment", CreationDate = DateTime.UtcNow, LastUpdateDate = DateTime.UtcNow, CreatedBy = 1, LastUpdatedBy = 1 }
        };
    }

    private static IEnumerable<ApInvoice> GetSeedInvoices()
    {
        return new List<ApInvoice>
        {
            new()
            {
                InvoiceNum = "INV-2026-001",
                InvoiceTypeLookupCode = "STANDARD",
                InvoiceDate = DateTime.UtcNow.ToString("o"),
                VendorId = 100,
                InvoiceAmount = "50000",
                InvoiceCurrencyCode = "INR",
                Description = "Travel Agency Services - March 2026",
                OrgId = 1,
                Status = "N",
                AgencyId = 1,
                CreationDate = DateTime.UtcNow.ToString("o"),
                CreatedBy = 1
            }
        };
    }

    private static IEnumerable<TravelBatchMain> GetSeedBatches()
    {
        return new List<TravelBatchMain>
        {
            new()
            {
                UnitCode = "001",
                BatchNumber = 1,
                BatchDate = DateTime.UtcNow,
                InvoiceNumber = "INV-2026-001",
                BatchStatus = "N",
                AdminRemarks = "Initial batch",
                AgencyCode = 1,
                Total = 50000
            }
        };
    }
}
