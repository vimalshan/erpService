using AdminService.Infrastructure.Persistence;
using AdminService.Domain.Entities;

namespace AdminService.Infrastructure;

/// <summary>
/// Database seeding service
/// </summary>
public class DatabaseSeeder
{
    public static async Task SeedAsync(AdminServiceDbContext context)
    {
        try
        {
            // Seed Admin Units
            if (!context.AdminUnits.Any())
            {
                var adminUnits = new List<AdminUnit>
                {
                    new AdminUnit
                    {
                        AdminCode = 1001,
                        Name = "Corporate Travel",
                        AdminType = "T",
                        UnitCode = "CRP",
                        ImageUrl = "https://example.com/corporate.jpg",
                        SortOrder = 1,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new AdminUnit
                    {
                        AdminCode = 1002,
                        Name = "Hotel Management",
                        AdminType = "S",
                        UnitCode = "HTL",
                        ImageUrl = "https://example.com/hotel.jpg",
                        SortOrder = 2,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new AdminUnit
                    {
                        AdminCode = 1003,
                        Name = "Meeting Coordination",
                        AdminType = "M",
                        UnitCode = "MTG",
                        ImageUrl = "https://example.com/meeting.jpg",
                        SortOrder = 3,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    }
                };

                await context.AdminUnits.AddRangeAsync(adminUnits);
            }

            // Seed Finance Units
            if (!context.FinanceUnits.Any())
            {
                var financeUnits = new List<FinanceUnit>
                {
                    new FinanceUnit
                    {
                        UnitId = 2001,
                        UnitCode = "FIN",
                        Name = "Finance Department",
                        OracleCode = 5001,
                        LocationOption = "N",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new FinanceUnit
                    {
                        UnitId = 2002,
                        UnitCode = "ACC",
                        Name = "Accounting Division",
                        OracleCode = 5002,
                        LocationOption = "N",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    }
                };

                await context.FinanceUnits.AddRangeAsync(financeUnits);
            }

            // Seed Area Masters
            if (!context.AreaMasters.Any())
            {
                var areas = new List<AreaMaster>
                {
                    new AreaMaster
                    {
                        AreaId = 3001,
                        AreaName = "North Region",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new AreaMaster
                    {
                        AreaId = 3002,
                        AreaName = "South Region",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new AreaMaster
                    {
                        AreaId = 3003,
                        AreaName = "East Region",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    }
                };

                await context.AreaMasters.AddRangeAsync(areas);
            }

            // Seed Route Masters
            if (!context.RouteMasters.Any())
            {
                var routes = new List<RouteMaster>
                {
                    new RouteMaster
                    {
                        RouteId = 4001,
                        RouteName = "Express Route",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new RouteMaster
                    {
                        RouteId = 4002,
                        RouteName = "Standard Route",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM"
                    }
                };

                await context.RouteMasters.AddRangeAsync(routes);
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error seeding database", ex);
        }
    }
}
