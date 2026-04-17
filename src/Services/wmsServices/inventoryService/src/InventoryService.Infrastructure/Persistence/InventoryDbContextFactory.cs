using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InventoryService.Infrastructure.Persistence;

public class InventoryDbContextFactory : IDesignTimeDbContextFactory<InventoryDbContext>
{
    public InventoryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WMSDB;Integrated Security=True;TrustServerCertificate=True";
        optionsBuilder.UseSqlServer(connectionString);

        return new InventoryDbContext(optionsBuilder.Options);
    }
}
