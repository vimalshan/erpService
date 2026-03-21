using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Enums;
using FleetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FleetManagement.Infrastructure;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
        await db.Database.MigrateAsync();

        if (await db.Vehicles.AnyAsync()) return;

        db.Vehicles.AddRange(
            new Vehicle { Code = "VH-001", LicensePlate = "ABC-1234", VehicleType = VehicleType.TRUCK, Make = "Volvo", Model = "FH16", Year = 2022, CapacityWeight = 25000, CapacityVolume = 80 },
            new Vehicle { Code = "VH-002", LicensePlate = "DEF-5678", VehicleType = VehicleType.VAN, Make = "Mercedes", Model = "Sprinter", Year = 2023, CapacityWeight = 3500, CapacityVolume = 14 },
            new Vehicle { Code = "VH-003", LicensePlate = "GHI-9012", VehicleType = VehicleType.FORKLIFT, Make = "Toyota", Model = "8FGU25", Year = 2021, CapacityWeight = 2500, CapacityVolume = 0 }
        );

        db.Drivers.AddRange(
            new Driver { Code = "DR-001", FullName = "John Smith", LicenseNumber = "DL-123456", LicenseExpiry = DateTime.UtcNow.AddYears(2), Phone = "555-0101", Email = "jsmith@fleet.com" },
            new Driver { Code = "DR-002", FullName = "Jane Doe", LicenseNumber = "DL-789012", LicenseExpiry = DateTime.UtcNow.AddYears(1), Phone = "555-0102", Email = "jdoe@fleet.com" }
        );

        db.Routes.AddRange(
            new Route { RouteName = "Warehouse-A to Customer-Hub", Description = "Main delivery route", StartLocation = "Warehouse A", EndLocation = "Customer Hub", EstimatedDuration = 120 },
            new Route { RouteName = "Inter-Warehouse Transfer", Description = "Between warehouses", StartLocation = "Warehouse A", EndLocation = "Warehouse B", EstimatedDuration = 60 }
        );

        await db.SaveChangesAsync();
    }
}
