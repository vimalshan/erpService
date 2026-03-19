using MediatR;
using Microsoft.EntityFrameworkCore;
using VehicleTracking.Domain.Common;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Infrastructure.Persistence;

public class VehicleTrackingDbContext(DbContextOptions<VehicleTrackingDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public DbSet<VehicleMaster> VehicleMasters => Set<VehicleMaster>();
    public DbSet<VehicleStage> VehicleStages => Set<VehicleStage>();
    public DbSet<VehicleTransaction> VehicleTransactions => Set<VehicleTransaction>();
    public DbSet<VehicleInvoice> VehicleInvoices => Set<VehicleInvoice>();
    public DbSet<VehicleDirectEntry> VehicleDirectEntries => Set<VehicleDirectEntry>();
    public DbSet<DecisionFlag> DecisionFlags => Set<DecisionFlag>();
    public DbSet<StageMaster> StageMasters => Set<StageMaster>();
    public DbSet<PurposeMaster> PurposeMasters => Set<PurposeMaster>();
    public DbSet<PurposeStage> PurposeStages => Set<PurposeStage>();
    public DbSet<PurposeProduct> PurposeProducts => Set<PurposeProduct>();
    public DbSet<StageDecision> StageDecisions => Set<StageDecision>();
    public DbSet<StageFlex> StageFlexes => Set<StageFlex>();
    public DbSet<SparshNavigation> SparshNavigations => Set<SparshNavigation>();
    public DbSet<WeightInformation> WeightInformations => Set<WeightInformation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VehicleTrackingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, ct);

        return result;
    }
}
