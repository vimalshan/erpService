using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ClubMembershipService.Infrastructure.Data;

public class ClubMembershipDbContextFactory : IDesignTimeDbContextFactory<ClubMembershipDbContext>
{
    public ClubMembershipDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "ClubMembershipService.API"))
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ClubMembershipDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new ClubMembershipDbContext(optionsBuilder.Options);
    }
}
