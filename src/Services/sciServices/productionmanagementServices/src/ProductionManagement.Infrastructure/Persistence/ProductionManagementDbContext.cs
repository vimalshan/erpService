using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Domain.Common;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Infrastructure.Persistence;

public class ProductionManagementDbContext : DbContext
{
    private readonly IMediator _mediator;

    public ProductionManagementDbContext(DbContextOptions<ProductionManagementDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<ProductionPlant> ProductionPlants => Set<ProductionPlant>();
    public DbSet<ProductionPlan> ProductionPlans => Set<ProductionPlan>();
    public DbSet<ProductionPlanEntry> ProductionPlanEntries => Set<ProductionPlanEntry>();
    public DbSet<ProductionPlantProductMap> ProductionPlantProductMaps => Set<ProductionPlantProductMap>();
    public DbSet<MamProductionDet> MamProductionDets => Set<MamProductionDet>();
    public DbSet<MamProductionMap> MamProductionMaps => Set<MamProductionMap>();
    public DbSet<NormsMain> NormsMain => Set<NormsMain>();
    public DbSet<NormsMaster> NormsMasters => Set<NormsMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductionManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
