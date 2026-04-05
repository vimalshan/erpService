namespace WebsiteContentService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using WebsiteContentService.Domain.Common;
using WebsiteContentService.Domain.Entities;
using WebsiteContentService.Infrastructure.Persistence.EntityConfigurations;

public class WebsiteContentDbContext : DbContext
{
    public DbSet<WebsitePage> WebsitePages { get; set; } = null!;
    public DbSet<WebsiteNews> WebsiteNews { get; set; } = null!;

    public WebsiteContentDbContext(DbContextOptions<WebsiteContentDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.ApplyConfiguration(new WebsitePageConfiguration());
        modelBuilder.ApplyConfiguration(new WebsiteNewsConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private Task DispatchDomainEventsAsync()
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        foreach (var aggregate in aggregateRoots)
            aggregate.ClearDomainEvents();

        return Task.CompletedTask;
    }
}
