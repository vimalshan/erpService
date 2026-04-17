using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FaqServices.Infrastructure.Data;

public class FaqDbContextFactory : IDesignTimeDbContextFactory<FaqDbContext>
{
    public FaqDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FaqDbContext>();
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\mssqllocaldb;Database=FaqDb;Trusted_Connection=true;";
        
        optionsBuilder.UseSqlServer(
            connectionString,
            x => x.MigrationsHistoryTable("__EFMigrationsHistory", "dbo")
        );

        return new FaqDbContext(optionsBuilder.Options);
    }
}
