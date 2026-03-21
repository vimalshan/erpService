using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TravelService.Domain.Common;
using TravelService.Domain.Entities;
using TravelService.Domain.Entities.Batch;
using TravelService.Domain.Entities.Forex;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence;

public class TravelDbContext : DbContext
{
    public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options) { }

    // TourPlan aggregate
    public DbSet<TourPlan> TourPlans => Set<TourPlan>();
    public DbSet<TourPlanAdvance> TourPlanAdvances => Set<TourPlanAdvance>();
    public DbSet<TourPlanAgenda> TourPlanAgendas => Set<TourPlanAgenda>();
    public DbSet<TourPlanCostCentre> TourPlanCostCentres => Set<TourPlanCostCentre>();
    public DbSet<TourPlanDaBreak> TourPlanDaBreaks => Set<TourPlanDaBreak>();
    public DbSet<TourPlanExpense> TourPlanExpenses => Set<TourPlanExpense>();
    public DbSet<TourPlanIntSchedule> TourPlanIntSchedules => Set<TourPlanIntSchedule>();
    public DbSet<TourPlanLeave> TourPlanLeaves => Set<TourPlanLeave>();
    public DbSet<TourPlanNmsSchedule> TourPlanNmsSchedules => Set<TourPlanNmsSchedule>();
    public DbSet<TourPlanSelfExpense> TourPlanSelfExpenses => Set<TourPlanSelfExpense>();

    // Batch aggregate
    public DbSet<BatchMain> BatchMains => Set<BatchMain>();
    public DbSet<BatchSub> BatchSubs => Set<BatchSub>();

    // Forex aggregate
    public DbSet<ForexMain> ForexMains => Set<ForexMain>();
    public DbSet<ForexDetail> ForexDetails => Set<ForexDetail>();
    public DbSet<ForexChequeDetail> ForexChequeDetails => Set<ForexChequeDetail>();

    // Other entities
    public DbSet<ApproverDetail> ApproverDetails => Set<ApproverDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TravelDbContext).Assembly);

        // Domain events are in-memory only — never persisted
        modelBuilder.Ignore<DomainEvent>();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var entitiesWithEvents = ChangeTracker.Entries<Entity<string>>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in entitiesWithEvents)
            entity.ClearDomainEvents();

        return result;
    }
}
