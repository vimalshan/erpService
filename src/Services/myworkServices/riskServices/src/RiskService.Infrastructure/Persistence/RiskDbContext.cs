using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RiskService.Domain.Aggregates;
using RiskService.Domain.Common;
using RiskService.Domain.Entities;
using RiskService.Domain.Interfaces;

namespace RiskService.Infrastructure.Persistence;

public class RiskDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public RiskDbContext(DbContextOptions<RiskDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    // Aggregates
    public DbSet<RiskAggregate> Risks => Set<RiskAggregate>();
    public DbSet<RiskMitigation> Mitigations => Set<RiskMitigation>();
    public DbSet<RiskSelfAssessment> SelfAssessments => Set<RiskSelfAssessment>();

    // Entities
    public DbSet<RiskType> RiskTypes => Set<RiskType>();
    public DbSet<RiskImpact> RiskImpacts => Set<RiskImpact>();
    public DbSet<RiskProbability> RiskProbabilities => Set<RiskProbability>();
    public DbSet<RiskRating> RiskRatings => Set<RiskRating>();
    public DbSet<RiskResponse> RiskResponses => Set<RiskResponse>();
    public DbSet<RiskDivision> RiskDivisions => Set<RiskDivision>();
    public DbSet<RiskDivisionUnit> RiskDivisionUnits => Set<RiskDivisionUnit>();
    public DbSet<RiskFunction> RiskFunctions => Set<RiskFunction>();
    public DbSet<RiskDivisionFunctionMap> DivisionFunctionMaps => Set<RiskDivisionFunctionMap>();
    public DbSet<RiskCause> RiskCauses => Set<RiskCause>();
    public DbSet<RiskControl> RiskControls => Set<RiskControl>();
    public DbSet<RiskImpactMap> RiskImpactMaps => Set<RiskImpactMap>();
    public DbSet<RiskEvent> RiskEvents => Set<RiskEvent>();
    public DbSet<RiskMonitor> RiskMonitors => Set<RiskMonitor>();
    public DbSet<RiskFrequencyMap> RiskFrequencyMaps => Set<RiskFrequencyMap>();
    public DbSet<RiskUnitChampion> RiskUnitChampions => Set<RiskUnitChampion>();
    public DbSet<RiskFunctionDetail> RiskFunctionDetails => Set<RiskFunctionDetail>();
    public DbSet<RiskUnitDetail> RiskUnitDetails => Set<RiskUnitDetail>();
    public DbSet<RiskApproval> RiskApprovals => Set<RiskApproval>();
    public DbSet<RiskMitigationAction> MitigationActions => Set<RiskMitigationAction>();
    public DbSet<RiskMitigationApproval> MitigationApprovals => Set<RiskMitigationApproval>();
    public DbSet<RiskSelfAssessmentComment> SelfAssessmentComments => Set<RiskSelfAssessmentComment>();
    public DbSet<RiskEventAssessment> EventAssessments => Set<RiskEventAssessment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RiskDbContext).Assembly);
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
