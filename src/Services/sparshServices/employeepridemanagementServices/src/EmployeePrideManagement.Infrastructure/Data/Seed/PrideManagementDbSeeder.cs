using EmployeePrideManagement.Domain.Entities;
using EmployeePrideManagement.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeePrideManagement.Infrastructure.Data.Seed;

public static class PrideManagementDbSeeder
{
    public static async Task SeedAsync(PrideManagementDbContext context, ILogger logger)
    {
        if (await context.MomentPrides.AnyAsync())
        {
            logger.LogInformation("Database already seeded.");
            return;
        }

        logger.LogInformation("Seeding Pride Management database...");

        var moments = new List<MomentPride>
        {
            new("Q1 Sales Target Achievement",
                "Our sales team exceeded Q1 targets by 25%, demonstrating exceptional teamwork and dedication.",
                1001m, "Celebrating Team Excellence", "Head Office, Conference Room A",
                "/images/pride/q1_achievement_2026.jpg", 1002),

            new("Employee of the Month - March 2026",
                "Recognizing outstanding contribution to the development team with innovative solutions.",
                1003m, "Individual Excellence Award", "Main Auditorium",
                "/images/pride/eom_march_2026.jpg", 1002),

            new("Safety Milestone - 500 Days Incident Free",
                "Our manufacturing unit has achieved 500 days without any safety incident.",
                1005m, "Safety First Initiative", "Plant Floor, Building B",
                "/images/pride/safety_500_2026.jpg", 1004),

            new("Project Delivery Excellence",
                "Successfully delivered the enterprise migration project 2 weeks ahead of schedule.",
                1007m, "Project Management Excellence", "IT Wing, 3rd Floor",
                "/images/pride/project_delivery_2026.jpg", 1006),

            new("Client Appreciation Award",
                "Received outstanding feedback from our key client for exceptional service delivery.",
                1009m, "Client Success Story", "Client Meeting Room",
                "/images/pride/client_appreciation_2026.jpg", 1008)
        };

        context.MomentPrides.AddRange(moments);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} pride moments.", moments.Count);
    }
}
