using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CanteenTransactionService.Infrastructure.Persistence.EF;

public class CanteenTransactionDbContextFactory : IDesignTimeDbContextFactory<CanteenTransactionDbContext>
{
    public CanteenTransactionDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CanteenTransactionDb;Integrated Security=True;TrustServerCertificate=True";
        var optionsBuilder = new DbContextOptionsBuilder<CanteenTransactionDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new CanteenTransactionDbContext(optionsBuilder.Options);
    }
}
