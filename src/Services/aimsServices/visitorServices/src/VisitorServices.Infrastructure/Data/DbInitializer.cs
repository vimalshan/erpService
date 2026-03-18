using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VisitorServices.Domain.Aggregates;
using VisitorServices.Infrastructure.Data;

namespace VisitorServices.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(VisitorDbContext context, ILogger logger)
    {
        await context.Database.MigrateAsync();

        if (await context.Visitors.AnyAsync())
        {
            logger.LogInformation("Database already seeded.");
            return;
        }

        logger.LogInformation("Seeding database...");

        // Seed visitors
        var visitor1 = VisitorAggregate.Register(
            id: 1,
            name: "Ahmed Al-Rashidi",
            idTypeChar: 'N',
            idNumber: "123456789",
            phone: "+966501234567",
            email: "ahmed@example.com",
            company: "TechCorp Ltd",
            purpose: "Meeting with HR Department",
            whomToVisit: 100,
            enteredBy: 1);
        visitor1.ClearDomainEvents();

        var visitor2 = VisitorAggregate.Register(
            id: 2,
            name: "Sara Mohamed",
            idTypeChar: 'P',
            idNumber: "A12345678",
            phone: "+966559876543",
            email: "sara@example.com",
            company: "Global Logistics",
            purpose: "Delivery & Documentation",
            whomToVisit: 101,
            enteredBy: 1);
        visitor2.ClearDomainEvents();

        await context.Visitors.AddRangeAsync(visitor1, visitor2);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeding complete. 2 visitors added.");
    }
}
