using Microsoft.EntityFrameworkCore;
using ItemMasterService.Domain.Entities;

namespace ItemMasterService.Infrastructure.Persistence.EF;

public class ItemMasterDbContext : DbContext
{
    public ItemMasterDbContext(DbContextOptions<ItemMasterDbContext> options) : base(options) { }

    public DbSet<CanteenItemMaster> CanteenItemMasters => Set<CanteenItemMaster>();
    public DbSet<CanteenItemPriceMaster> CanteenItemPriceMasters => Set<CanteenItemPriceMaster>();
    public DbSet<CanteenGradeItemPrice> CanteenGradeItemPrices => Set<CanteenGradeItemPrice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItemMasterDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker.Entries<Domain.Common.AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.Entity.DomainEvents).ToList();
        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // Fire-and-forget inner domain events (handled separately via MediatR in DomainEventDispatcher)
        return result;
    }
}
