using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TravelService.Domain.Entities.TourPlan;
using TravelService.Domain.ValueObjects;

namespace TravelService.Infrastructure.Persistence;

public static class TravelDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger logger)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TravelDbContext>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.TourPlans.AnyAsync())
            {
                logger.LogInformation("Seeding TourPlan data...");

                var tp1 = TourPlan.Create(
                    id: "TP20260309001",
                    employeeSysId: "EMP001",
                    startDate: new DateTime(2026, 3, 10),
                    endDate: new DateTime(2026, 3, 14),
                    purpose: "Client Meeting for Q1 Business Review",
                    remarks: "Discuss quarterly review and new projects",
                    category: "DOM",
                    includeBooking: true,
                    fromCity: new CityInfo("MUM", "Mumbai"),
                    toCity: new CityInfo("DEL", "New Delhi"),
                    supervisorRemarks: "Approved by manager",
                    createdBy: "EMP001",
                    payrollUnitId: "PU001",
                    tripType: "Single City",
                    gradeType: "Grade A",
                    contactNo: "9876543210"
                );

                var tp2 = TourPlan.Create(
                    id: "TP20260309002",
                    employeeSysId: "EMP002",
                    startDate: new DateTime(2026, 3, 15),
                    endDate: new DateTime(2026, 3, 20),
                    purpose: "International Conference - Technology Summit",
                    remarks: "Attend Tech Summit 2026 and explore partnerships",
                    category: "INT",
                    includeBooking: true,
                    fromCity: new CityInfo("MUM", "Mumbai", "IND", "India"),
                    toCity: new CityInfo("NYC", "New York", "USA", "United States"),
                    supervisorRemarks: "CEO approved international travel",
                    createdBy: "EMP002",
                    payrollUnitId: "PU002",
                    tripType: "Multiple City",
                    gradeType: "Grade B",
                    contactNo: "9876543211"
                );

                await context.TourPlans.AddRangeAsync(tp1, tp2);
                await context.SaveChangesAsync();
                logger.LogInformation("TourPlan seed data added.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
