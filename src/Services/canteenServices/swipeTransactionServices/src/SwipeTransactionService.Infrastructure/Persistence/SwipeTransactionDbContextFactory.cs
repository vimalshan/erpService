using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SwipeTransactionService.Infrastructure.Persistence;

/// <summary>Used only by EF Core design-time tools (dotnet ef migrations add).</summary>
public sealed class SwipeTransactionDbContextFactory : IDesignTimeDbContextFactory<SwipeTransactionDbContext>
{
    public SwipeTransactionDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "SwipeTransactionService.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<SwipeTransactionDbContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        return new SwipeTransactionDbContext(optionsBuilder.Options);
    }
}
