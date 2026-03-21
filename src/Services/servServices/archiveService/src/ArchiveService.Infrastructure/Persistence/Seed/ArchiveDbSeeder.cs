using ArchiveService.Domain.Entities;
using ArchiveService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArchiveService.Infrastructure.Persistence.Seed;

public static class ArchiveDbSeeder
{
    public static async Task SeedAsync(ArchiveDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.ArchivedServiceOrders.AnyAsync())
            return;

        var order = ArchivedServiceOrder.Create(
            sernoDell: "SEED000001",
            branch: "HYD",
            sapLogin: "SAPUSER01",
            postingDate: new DateTime(2024, 1, 15),
            sapId: "SAP00000001",
            sla: "NBD",
            productId: "LAT-5520",
            serviceTag: "ABCD1234567",
            relatedCase: "CASE001",
            lob: "CLIENT",
            callStatus: "COMPLETED",
            currentRc: "RC01",
            engineerId: "ENG001",
            engineerName: "John Doe",
            engMobNo: "9876543210",
            orgName: "Acme Corp",
            customerName: "Jane Smith",
            contactNo: "1234567890",
            address: "123 Main St, Hyderabad",
            altCntNo: "0987654321",
            dispatchDate: new DateTime(2024, 1, 14),
            custEtaDate: new DateTime(2024, 1, 15),
            partEtaDate: new DateTime(2024, 1, 15),
            techSupName: "Tech Support Team",
            dsp: "DSP-HYD-01",
            prbDesc: "Hard disk failure",
            longDesc: "Customer reported hard disk failure. Replaced with new SSD.",
            reasonCode: "HW-FAIL",
            activity: "PART_REPLACE",
            onsiteDt: new DateTime(2024, 1, 15, 10, 0, 0),
            cmpltdDt: new DateTime(2024, 1, 15, 12, 0, 0),
            flag: "C",
            enteredBy: "SYSTEM");

        order.ClearDomainEvents();

        var detail = ArchivedServiceOrderDetail.Create(
            sernoDell: "SEED000001",
            partNo: "SSD-512GB",
            quantity: "1",
            uniqueId: "UID001",
            partStatus: "USED",
            enteredBy: "SYSTEM");

        order.AddDetail(detail);
        context.ArchivedServiceOrders.Add(order);

        var toolkit = ArchivedToolKit.Create(
            kitCode: "KIT001",
            appPassword: "app123",
            instPassword: "inst123",
            imeiNo: "123456789012345",
            engineerId: "ENG001",
            flag: "A",
            enteredBy: "SYSTEM");

        toolkit.ClearDomainEvents();
        context.ArchivedToolKits.Add(toolkit);

        await context.SaveChangesAsync();
    }
}
