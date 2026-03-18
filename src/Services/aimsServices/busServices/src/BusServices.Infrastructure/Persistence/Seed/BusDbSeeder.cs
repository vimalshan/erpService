using BusServices.Domain.Entities;
using BusServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusServices.Infrastructure.Persistence.Seed;

public sealed class BusDbSeeder
{
    private readonly BusDbContext _ctx;
    private readonly ILogger<BusDbSeeder> _logger;

    public BusDbSeeder(BusDbContext ctx, ILogger<BusDbSeeder> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        if (await _ctx.Buses.AnyAsync(ct))
        {
            _logger.LogInformation("Database already seeded. Skipping.");
            return;
        }

        _logger.LogInformation("Seeding BusDB with sample data...");

        // Buses
        var bus1 = Bus.Register(1, "ABC-1234", "Company Shuttle Bus A", 45, 1);
        var bus2 = Bus.Register(2, "XYZ-5678", "Company Shuttle Bus B", 50, 1);
        var bus3 = Bus.Register(3, "DEF-9012", "Executive Shuttle", 20, 1);

        await _ctx.Buses.AddRangeAsync([bus1, bus2, bus3], ct);
        await _ctx.SaveChangesAsync(ct);

        // Routes
        var route1 = bus1.AddRoute(1, "North Zone - Morning", "North zone pickup, 7:30 AM", 1);
        var route2 = bus1.AddRoute(2, "North Zone - Evening", "North zone drop, 6:00 PM", 1);
        var route3 = bus2.AddRoute(3, "South Zone - Morning", "South zone pickup, 8:00 AM", 1);
        var route4 = bus2.AddRoute(4, "South Zone - Evening", "South zone drop, 5:30 PM", 1);
        var route5 = bus3.AddRoute(5, "Executive Route", "VIP Executive shuttle route", 1);

        await _ctx.BusRoutes.AddRangeAsync([route1, route2, route3, route4, route5], ct);
        await _ctx.SaveChangesAsync(ct);

        // Employee Assignments
        var assignments = new[]
        {
            EmployeeBus.Assign(1, 1001, 1, 1, 1),
            EmployeeBus.Assign(2, 1002, 1, 1, 1),
            EmployeeBus.Assign(3, 1003, 2, 3, 1),
            EmployeeBus.Assign(4, 1004, 2, 3, 1),
            EmployeeBus.Assign(5, 1005, 3, 5, 1)
        };

        await _ctx.EmployeeBusAssignments.AddRangeAsync(assignments, ct);
        await _ctx.SaveChangesAsync(ct);

        // Deduction Rates
        var rates = new[]
        {
            BusDeductionRate.Create(1, 1, 250.00m, new DateTime(2026, 1, 1), 1),
            BusDeductionRate.Create(2, 2, 250.00m, new DateTime(2026, 1, 1), 1),
            BusDeductionRate.Create(3, 3, 150.00m, new DateTime(2026, 1, 1), 1)
        };

        await _ctx.BusDeductionRates.AddRangeAsync(rates, ct);

        // Arrival Records
        var today = DateTime.UtcNow.Date;
        var arrivals = new[]
        {
            bus1.RecordArrival(1, today, new TimeOnly(7, 45), 'O', "On time", 1),
            bus2.RecordArrival(2, today, new TimeOnly(8, 05), 'L', "5 mins late", 1),
            bus3.RecordArrival(3, today, new TimeOnly(7, 30), 'E', "Early arrival", 1)
        };

        await _ctx.BusArrivals.AddRangeAsync(arrivals, ct);
        await _ctx.SaveChangesAsync(ct);

        _logger.LogInformation("Seed completed. Buses: 3, Routes: 5, Assignments: 5, DeductionRates: 3, Arrivals: 3.");
    }
}
