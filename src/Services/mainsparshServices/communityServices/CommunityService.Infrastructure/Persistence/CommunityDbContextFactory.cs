using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CommunityService.Infrastructure.Persistence;

public class CommunityDbContextFactory : IDesignTimeDbContextFactory<CommunityDbContext>
{
    public CommunityDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SRFSPARSHDB;Integrated Security=True;TrustServerCertificate=True;Connection Timeout=30;";
        
        var optionsBuilder = new DbContextOptionsBuilder<CommunityDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        
        return new CommunityDbContext(optionsBuilder.Options);
    }
}
