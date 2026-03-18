using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MediatR;
using Moq;

namespace EmployeeService.Infrastructure.Persistence;

/// <summary>
/// Design-time factory — allows EF migrations CLI to instantiate EmployeeDbContext
/// without needing the full application host.
/// </summary>
public sealed class EmployeeDbContextFactory : IDesignTimeDbContextFactory<EmployeeDbContext>
{
    public EmployeeDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../EmployeeService.API"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<EmployeeDbContext>();
        optionsBuilder.UseSqlServer(
            config.GetConnectionString("EmployeeDb"),
            sql => sql.MigrationsAssembly(typeof(EmployeeDbContext).Assembly.FullName));

        // Use a no-op mediator for design-time — domain events don't fire during migrations
        var mediatorMock = new Mock<IMediator>();
        return new EmployeeDbContext(optionsBuilder.Options, mediatorMock.Object);
    }
}
