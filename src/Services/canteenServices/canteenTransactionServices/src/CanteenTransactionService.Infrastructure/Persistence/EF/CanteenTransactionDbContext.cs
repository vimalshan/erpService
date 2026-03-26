using Microsoft.EntityFrameworkCore;
using CanteenTransactionService.Domain.Entities;

namespace CanteenTransactionService.Infrastructure.Persistence.EF;

public class CanteenTransactionDbContext : DbContext
{
    public CanteenTransactionDbContext(DbContextOptions<CanteenTransactionDbContext> options) : base(options) { }

    public DbSet<CanteenDacon> CanteenDacons => Set<CanteenDacon>();
    public DbSet<DailyAvailed> DailyAvaileds => Set<DailyAvailed>();
    public DbSet<MisBatchSubmission> MisBatchSubmissions => Set<MisBatchSubmission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CanteenTransactionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<Domain.Common.AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.Entity.DomainEvents).ToList();
        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        return await base.SaveChangesAsync(cancellationToken);
    }
}
