using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace ApprovalGroup.Infrastructure.Persistence;

/// <summary>
/// Used by EF Core tools (dotnet ef migrations add) at design time
/// </summary>
public class ApprovalGroupDbContextFactory : IDesignTimeDbContextFactory<ApprovalGroupDbContext>
{
    public ApprovalGroupDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApprovalGroupDbContext>();
        optionsBuilder.UseSqlServer(
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SSCDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");

        // Provide logging before MediatR registration for design-time factory
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApprovalGroupDbContextFactory).Assembly));
        var sp = services.BuildServiceProvider();
        var mediator = sp.GetRequiredService<IMediator>();

        return new ApprovalGroupDbContext(optionsBuilder.Options, mediator);
    }
}
