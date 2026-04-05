namespace WebsiteContentService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class WebsiteContentDbContextFactory : IDesignTimeDbContextFactory<WebsiteContentDbContext>
{
    public WebsiteContentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WebsiteContentDbContext>();
        optionsBuilder.UseSqlServer(
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SRFSPARSHDB;Integrated Security=True;TrustServerCertificate=True");

        return new WebsiteContentDbContext(optionsBuilder.Options);
    }
}
