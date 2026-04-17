using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookingService.Infrastructure.Persistence;

public class BookingDbContextFactory : IDesignTimeDbContextFactory<BookingDbContext>
{
    public BookingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookingDbContext>();
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TOURDB;Integrated Security=True;TrustServerCertificate=True";
        optionsBuilder.UseSqlServer(connectionString);

        // Use a null mediator for design-time operations
        return new BookingDbContext(optionsBuilder.Options, null!);
    }
}
