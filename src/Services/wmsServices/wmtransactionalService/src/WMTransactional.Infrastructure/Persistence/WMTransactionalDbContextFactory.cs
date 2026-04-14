using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WMTransactional.Infrastructure.Persistence;

public class WMTransactionalDbContextFactory : IDesignTimeDbContextFactory<WMTransactionalDbContext>
{
    public WMTransactionalDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WMTransactionalDbContext>();
        optionsBuilder.UseSqlServer(
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WMTransactionalDb;Integrated Security=True;TrustServerCertificate=True");

        return new WMTransactionalDbContext(optionsBuilder.Options);
    }
}
