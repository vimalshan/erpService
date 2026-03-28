using Microsoft.EntityFrameworkCore;
using AuthorizationService.Domain.Entities;
using AuthorizationService.Infrastructure.Data;

namespace AuthorizationService.Infrastructure;

public static class SeedDataExtensions
{
    public static async Task SeedAuthorizationDataAsync(this AuthorizationDbContext context)
    {
        // Seed Rights
        if (!context.Rights.Any())
        {
            var rights = new List<Right>
            {
                new Right(1, "ADM") { },
                new Right(2, "VIW") { },
                new Right(3, "EDT") { }
            };

            await context.Rights.AddRangeAsync(rights);
            await context.SaveChangesAsync();
        }

        // Seed UserRights
        if (!context.UserRights.Any())
        {
            var userRights = new List<UserRight>
            {
                new UserRight("admin@company.com", 1001, 1)
                {
                    BusinessCode = "BUS001",
                    UnitCode = "UN001",
                    RightMode = 1
                },
                new UserRight("manager@company.com", 1002, 2)
                {
                    BusinessCode = "BUS001",
                    UnitCode = "UN001",
                    RightMode = 2
                },
                new UserRight("user@company.com", 1003, 3)
                {
                    BusinessCode = "BUS002",
                    UnitCode = "UN002",
                    RightMode = 3
                }
            };

            await context.UserRights.AddRangeAsync(userRights);
            await context.SaveChangesAsync();
        }

        // Seed TrackerRights
        if (!context.TrackerRights.Any())
        {
            var trackerRights = new List<TrackerRight>
            {
                new TrackerRight("admin@company.com", 1001, "BUS001")
                {
                    TrackerMode = "ADM",
                    UnitCode = "UN1",
                    TrackerRights = 'Y',
                    VtcRights = 'Y',
                    RepresentingUnit = 'Y',
                    LetRight = 'Y',
                    CarRight = 'N'
                },
                new TrackerRight("manager@company.com", 1002, "BUS001")
                {
                    TrackerMode = "MGR",
                    UnitCode = "UN1",
                    TrackerRights = 'Y',
                    VtcRights = 'N',
                    RepresentingUnit = 'N',
                    LetRight = 'Y',
                    CarRight = 'N'
                }
            };

            await context.TrackerRights.AddRangeAsync(trackerRights);
            await context.SaveChangesAsync();
        }

        // Seed SpecialInputMasters
        if (!context.SpecialInputMasters.Any())
        {
            var masters = new List<SpecialInputMaster>
            {
                new SpecialInputMaster(1, 2025, "APPRAISER", 1001, 2001)
                {
                    CreatedBy = 1001,
                    CreatedOn = DateTime.UtcNow
                },
                new SpecialInputMaster(2, 2025, "REVIEWER", 1002, 2002)
                {
                    CreatedBy = 1002,
                    CreatedOn = DateTime.UtcNow
                }
            };

            await context.SpecialInputMasters.AddRangeAsync(masters);
            await context.SaveChangesAsync();
        }

        // Seed SpecialInputs
        if (!context.SpecialInputs.Any())
        {
            var specialInputs = new List<SpecialInput>
            {
                new SpecialInput(1, 2025, "APPRAISER", 1001, 2001)
                {
                    Inputs = "Excellent performance in Q1 targets",
                    Status = 'S',
                    CreatedOn = DateTime.UtcNow,
                    SubmittedOn = DateTime.UtcNow
                },
                new SpecialInput(2, 2025, "REVIEWER", 1002, 2002)
                {
                    Inputs = "Pending review for Q2 assessment",
                    Status = 'P',
                    CreatedOn = DateTime.UtcNow
                }
            };

            await context.SpecialInputs.AddRangeAsync(specialInputs);
            await context.SaveChangesAsync();
        }
    }
}
