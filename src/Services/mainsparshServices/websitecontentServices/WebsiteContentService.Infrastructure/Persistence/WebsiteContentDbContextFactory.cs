using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebsiteContentService.Infrastructure.Persistence;

public class WebsiteContentDbContextFactory : IDesignTimeDbContextFactory<WebsiteContentDbContext>
{
    public WebsiteContentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WebsiteContentDbContext>();
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SRFSPARSHDB;Integrated Security=True;TrustServerCertificate=True";
        optionsBuilder.UseSqlServer(connectionString);

        return new WebsiteContentDbContext(optionsBuilder.Options);
    }
}
