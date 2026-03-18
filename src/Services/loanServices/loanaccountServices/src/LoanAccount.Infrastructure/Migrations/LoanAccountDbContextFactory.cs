using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using LoanAccount.Infrastructure.Persistence;

namespace LoanAccount.Infrastructure.Migrations;

/// <summary>
/// EF Core design-time factory for DbContext
/// </summary>
public class LoanAccountDbContextFactory : IDesignTimeDbContextFactory<LoanAccountDbContext>
{
    public LoanAccountDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LoanAccountDbContext>();

        // Get connection string from appsettings.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("LoanAccountDb")
            ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LoanAccountDb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"SQL Server Management Studio\";Command Timeout=0";

        optionsBuilder.UseSqlServer(
            connectionString,
            sqlOptions => sqlOptions.MigrationsAssembly("LoanAccount.Infrastructure"));

        return new LoanAccountDbContext(optionsBuilder.Options);
    }
}
