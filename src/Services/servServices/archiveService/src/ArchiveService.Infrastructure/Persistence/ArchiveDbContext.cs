using ArchiveService.Domain.Common;
using ArchiveService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArchiveService.Infrastructure.Persistence;

public class ArchiveDbContext(DbContextOptions<ArchiveDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public DbSet<ArchivedServiceOrder> ArchivedServiceOrders => Set<ArchivedServiceOrder>();
    public DbSet<ArchivedServiceOrderDetail> ArchivedServiceOrderDetails => Set<ArchivedServiceOrderDetail>();
    public DbSet<ArchivedToolKit> ArchivedToolKits => Set<ArchivedToolKit>();
    public DbSet<ArchivedToolKitTransaction> ArchivedToolKitTransactions => Set<ArchivedToolKitTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArchiveDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}
