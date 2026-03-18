using Microsoft.EntityFrameworkCore;
using TeamServices.Domain.Entities;
using TeamServices.Infrastructure.Data;

namespace TeamServices.Infrastructure.Data;

public static class TeamDbContextSeed
{
    public static async Task SeedAsync(TeamDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Teams.AnyAsync())
        {
            var teams = new List<TeamMaster>
            {
                new(1, "Engineering Team", 1),
                new(2, "Marketing Team", 1),
                new(3, "Sales Team", 1),
                new(4, "HR Team", 1),
                new(5, "Finance Team", 1)
            };

            // Clear domain events from seeded entities
            foreach (var team in teams)
                team.ClearDomainEvents();

            await context.Teams.AddRangeAsync(teams);

            var employeeMaps = new List<TeamEmployeeMap>
            {
                new(1, 1, 1001, DateTime.UtcNow.AddMonths(-6), null, 1),
                new(2, 1, 1002, DateTime.UtcNow.AddMonths(-3), null, 1),
                new(3, 2, 1003, DateTime.UtcNow.AddMonths(-12), null, 1),
                new(4, 3, 1004, DateTime.UtcNow.AddMonths(-1), null, 1),
                new(5, 4, 1005, DateTime.UtcNow.AddMonths(-9), null, 1)
            };

            await context.TeamEmployeeMaps.AddRangeAsync(employeeMaps);

            var unitMaps = new List<TeamUnitMap>
            {
                new(1, 1, 100, 'A', null, 1),
                new(2, 2, 200, 'B', null, 1),
                new(3, 3, 300, 'A', 10, 1),
                new(4, 4, 400, 'C', null, 1),
                new(5, 5, 500, 'B', 20, 1)
            };

            await context.TeamUnitMaps.AddRangeAsync(unitMaps);
            await context.SaveChangesAsync();
        }
    }
}
