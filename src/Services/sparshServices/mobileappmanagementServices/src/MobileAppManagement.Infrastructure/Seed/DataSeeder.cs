using Microsoft.EntityFrameworkCore;
using MobileAppManagement.Domain.Entities;
using MobileAppManagement.Infrastructure.Persistence;

namespace MobileAppManagement.Infrastructure.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(MobileAppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.AppDeviceDetails.AnyAsync())
        {
            context.AppDeviceDetails.AddRange(
                AppDeviceDetail.Create(1001, "DEVICE_001", "A", "123456789012345", 1001),
                AppDeviceDetail.Create(1002, "DEVICE_002", "I", "987654321098765", 1002),
                AppDeviceDetail.Create(1003, "DEVICE_003", "A", "111222333444555", 1003)
            );
        }

        if (!await context.AppRegistrations.AnyAsync())
        {
            var reg1 = AppRegistration.Create(1, 1001, "user1@company.com", 2001, "E",
                "+911234567890", "123456789012345", "DEVICE_001", "A");
            reg1.GeneratePin(123456);
            reg1.MarkRegistered();

            var reg2 = AppRegistration.Create(2, 1002, "user2@company.com", 2002, "E",
                "+919876543210", "987654321098765", "DEVICE_002", "I");
            reg2.GeneratePin(654321);

            context.AppRegistrations.AddRange(reg1, reg2);
        }

        await context.SaveChangesAsync();
    }
}
