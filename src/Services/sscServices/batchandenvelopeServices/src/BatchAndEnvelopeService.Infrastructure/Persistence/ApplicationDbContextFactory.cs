using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace BatchAndEnvelopeService.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SSCDB;Integrated Security=True;TrustServerCertificate=True;");

        // Provide a no-op mediator for design-time migrations
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationDbContextFactory).Assembly));
        var sp = services.BuildServiceProvider();
        var mediator = sp.GetRequiredService<IMediator>();

        return new ApplicationDbContext(optionsBuilder.Options, mediator);
    }
}
