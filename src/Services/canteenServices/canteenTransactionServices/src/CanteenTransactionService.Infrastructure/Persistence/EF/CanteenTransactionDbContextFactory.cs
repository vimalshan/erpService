using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CanteenTransactionService.Infrastructure.Persistence.EF;

public class CanteenTransactionDbContextFactory : IDesignTimeDbContextFactory<CanteenTransactionDbContext>
{
    public CanteenTransactionDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CanteenTransactionDbContext>();
        optionsBuilder.UseSqlServer(
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CanteenTransactionDb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=CanteenTransactionService");

        return new CanteenTransactionDbContext(optionsBuilder.Options);
    }
}
