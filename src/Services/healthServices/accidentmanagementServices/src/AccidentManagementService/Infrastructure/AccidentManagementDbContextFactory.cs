using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using AccidentManagementService.Infrastructure.Persistence;

namespace AccidentManagementService.Infrastructure;

/// <summary>
/// Design-time factory for AccidentManagementDbContext
/// Required for EF Core migrations and scaffolding
/// </summary>
public class AccidentManagementDbContextFactory : IDesignTimeDbContextFactory<AccidentManagementDbContext>
{
    public AccidentManagementDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AccidentManagementDbContext>();
        
        // Use LocalDB for migrations
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\MSSQLLocalDB;Database=HEALTHDB;Integrated Security=True;",
            b => b.MigrationsAssembly("AccidentManagementService"));

        return new AccidentManagementDbContext(optionsBuilder.Options);
    }
}
