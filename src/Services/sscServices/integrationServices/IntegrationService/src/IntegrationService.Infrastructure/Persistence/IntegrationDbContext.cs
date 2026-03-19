using IntegrationService.Domain.Common;
using IntegrationService.Domain.Entities;
using IntegrationService.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.Infrastructure.Persistence;

public class IntegrationDbContext(
    DbContextOptions<IntegrationDbContext> options,
    IMediator mediator) : DbContext(options), IUnitOfWork
{
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<MaterialReceiptCertificate> MaterialReceipts => Set<MaterialReceiptCertificate>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<VendorSite> VendorSites => Set<VendorSite>();
    public DbSet<VendorSiteBuMapping> VendorSiteBuMappings => Set<VendorSiteBuMapping>();
    public DbSet<OrganizationUnit> OrganizationUnits => Set<OrganizationUnit>();
    public DbSet<OuBuMapping> OuBuMappings => Set<OuBuMapping>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IntegrationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity<long>>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEntitiesString = ChangeTracker.Entries<BaseEntity<string>>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEntitiesInt = ChangeTracker.Entries<BaseEntity<int>>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents)
            .Concat(domainEntitiesString.SelectMany(e => e.DomainEvents))
            .Concat(domainEntitiesInt.SelectMany(e => e.DomainEvents))
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());
        domainEntitiesString.ForEach(e => e.ClearDomainEvents());
        domainEntitiesInt.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
