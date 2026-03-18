using ProxyModule.Domain.Entities;
using ProxyModule.Infrastructure.Persistence;

namespace ProxyModule.Infrastructure.Seed;

public static class ProxyModuleSeeder
{
    public static async Task SeedAsync(ProxyModuleDbContext context)
    {
        if (context.ProxyRights.Any())
            return;

        var seedData = new List<ProxyRight>
        {
            ProxyRight.Create(
                proxyUserId: 100,
                delegatedUserId: 101,
                proxyStartDate: DateTime.UtcNow,
                proxyEndDate: DateTime.UtcNow.AddDays(30),
                proxyType: "APPROVAL",
                scope: "DEPARTMENT",
                notes: "Approval delegation for Q1 reviews",
                createdBy: 1),

            ProxyRight.Create(
                proxyUserId: 200,
                delegatedUserId: 201,
                proxyStartDate: DateTime.UtcNow,
                proxyEndDate: DateTime.UtcNow.AddDays(60),
                proxyType: "SUBMISSION",
                scope: "ALL",
                notes: "Submission delegation during leave",
                createdBy: 1),

            ProxyRight.Create(
                proxyUserId: 300,
                delegatedUserId: 301,
                proxyStartDate: DateTime.UtcNow,
                proxyEndDate: null,
                proxyType: "FULL",
                scope: "LOCATION",
                notes: "Permanent full proxy for branch office",
                createdBy: 1),

            ProxyRight.Create(
                proxyUserId: 400,
                delegatedUserId: 401,
                proxyStartDate: DateTime.UtcNow,
                proxyEndDate: DateTime.UtcNow.AddDays(7),
                proxyType: "READONLY",
                scope: "SPECIFIC",
                notes: "Temporary read-only access for audit",
                createdBy: 1)
        };

        context.ProxyRights.AddRange(seedData);
        await context.SaveChangesAsync();
    }
}
