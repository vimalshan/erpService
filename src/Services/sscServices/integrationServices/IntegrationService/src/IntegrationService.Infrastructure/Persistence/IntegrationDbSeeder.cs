using IntegrationService.Domain.Entities;
using IntegrationService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationService.Infrastructure.Persistence;

public static class IntegrationDbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IntegrationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<IntegrationDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.Vendors.AnyAsync())
            {
                var vendor1 = Vendor.Create(1001, "Acme Corporation", "ACME001");
                var vendor2 = Vendor.Create(1002, "Global Supplies Ltd", "GLBS002");
                context.Vendors.AddRange(vendor1, vendor2);

                // Clear domain events since we don't need them during seeding
                vendor1.ClearDomainEvents();
                vendor2.ClearDomainEvents();
            }

            if (!await context.OrganizationUnits.AnyAsync())
            {
                var ou1 = OrganizationUnit.Create("OU001", "Finance Department", "BU001");
                var ou2 = OrganizationUnit.Create("OU002", "Procurement Division", "BU002");
                context.OrganizationUnits.AddRange(ou1, ou2);
                ou1.ClearDomainEvents();
                ou2.ClearDomainEvents();
            }

            if (!await context.VendorSites.AnyAsync())
            {
                var site1 = VendorSite.Create(2001, 1001, "ACME-SITE-01", "OU001");
                var site2 = VendorSite.Create(2002, 1002, "GLBS-SITE-01", "OU002");
                context.VendorSites.AddRange(site1, site2);
            }

            if (!await context.PurchaseOrders.AnyAsync())
            {
                var po1 = PurchaseOrder.Create(1, 100, 5001, "PO-2026-001", 2001, new PaymentTerms(30, 0, 0));
                var po2 = PurchaseOrder.Create(2, 100, 5002, "PO-2026-002", 2002, new PaymentTerms(60, 15, 1));
                context.PurchaseOrders.AddRange(po1, po2);
                po1.ClearDomainEvents();
                po2.ClearDomainEvents();
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Database seeded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database");
            throw;
        }
    }
}
