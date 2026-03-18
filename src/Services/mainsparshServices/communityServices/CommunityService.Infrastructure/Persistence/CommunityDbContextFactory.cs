using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CommunityService.Infrastructure.Persistence;

public class CommunityDbContextFactory : IDesignTimeDbContextFactory<CommunityDbContext>
{
    public CommunityDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SRFSPARSHDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        var optionsBuilder = new DbContextOptionsBuilder<CommunityDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        
        return new CommunityDbContext(optionsBuilder.Options);
    }
}
