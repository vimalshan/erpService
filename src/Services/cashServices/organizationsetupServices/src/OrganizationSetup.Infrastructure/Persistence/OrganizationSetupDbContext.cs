using MediatR;
using Microsoft.EntityFrameworkCore;
using OrganizationSetup.Domain.Common;
using OrganizationSetup.Domain.Entities;
using OrganizationSetup.Infrastructure.Persistence.Configurations;

namespace OrganizationSetup.Infrastructure.Persistence;

public class OrganizationSetupDbContext : DbContext
{
    private readonly IMediator _mediator;

    public OrganizationSetupDbContext(DbContextOptions<OrganizationSetupDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<DealRole> DealRoles => Set<DealRole>();
    public DbSet<DealUserMap> DealUserMaps => Set<DealUserMap>();
    public DbSet<DealOrgParams> DealOrgParams => Set<DealOrgParams>();
    public DbSet<DealPpLimit> DealPpLimits => Set<DealPpLimit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new DealRoleConfiguration());
        modelBuilder.ApplyConfiguration(new DealUserMapConfiguration());
        modelBuilder.ApplyConfiguration(new DealOrgParamsConfiguration());
        modelBuilder.ApplyConfiguration(new DealPpLimitConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = entries
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Dispatch domain events after successful save
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entry in entries)
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
