using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MasterData.Domain.Entities;
using MasterData.Infrastructure.Persistence;

#nullable enable

namespace MasterData.Infrastructure
{
    /// <summary>
    /// Seed data script for initial data population
    /// </summary>
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(MasterDataDbContext context)
        {
            // Seed Company Units
            if (!context.CompanyUnits.Any())
            {
                var units = new[]
                {
                    CompanyUnitAggregate.Create("HQ", "Head Quarter"),
                    CompanyUnitAggregate.Create("BR1", "Branch 1"),
                    CompanyUnitAggregate.Create("BR2", "Branch 2")
                };

                context.CompanyUnits.AddRange(units);
            }

            // Seed Locations
            if (!context.Locations.Any())
            {
                var locations = new[]
                {
                    LocationAggregate.Create("New York"),
                    LocationAggregate.Create("Los Angeles"),
                    LocationAggregate.Create("Chicago")
                };

                context.Locations.AddRange(locations);
            }

            // Seed States
            if (!context.States.Any())
            {
                var states = new[]
                {
                    StateAggregate.Create("NY", "New York"),
                    StateAggregate.Create("CA", "California"),
                    StateAggregate.Create("IL", "Illinois"),
                    StateAggregate.Create("TX", "Texas")
                };

                context.States.AddRange(states);
            }

            // Seed Cities
            if (!context.Cities.Any())
            {
                var cities = new[]
                {
                    CityAggregate.Create("NYC", "New York City", "NY"),
                    CityAggregate.Create("LA", "Los Angeles", "CA"),
                    CityAggregate.Create("CHI", "Chicago", "IL"),
                    CityAggregate.Create("HOU", "Houston", "TX")
                };

                context.Cities.AddRange(cities);
            }

            // Seed Suppliers
            if (!context.Suppliers.Any())
            {
                var suppliers = new[]
                {
                    SupplierAggregate.Create("SUP001", "Supplier One Inc", "Quality supplier", "USER001", 100),
                    SupplierAggregate.Create("SUP002", "Supplier Two Ltd", "Reliable supplier", "USER002", 200),
                    SupplierAggregate.Create("SUP003", "Supplier Three Corp", null, "USER003", 300)
                };

                context.Suppliers.AddRange(suppliers);
            }

            await context.SaveChangesAsync();
        }
    }
}
