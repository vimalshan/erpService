using MasterService.Domain.Common;
using MasterService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MasterService.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<TrainingProvider> TrainingProviders => Set<TrainingProvider>();
    public DbSet<JobMaster> JobMasters => Set<JobMaster>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CompanyFinancialYear> CompanyFinancialYears => Set<CompanyFinancialYear>();
    public DbSet<Benefit> Benefits => Set<Benefit>();
    public DbSet<CostMaster> CostMasters => Set<CostMaster>();
    public DbSet<FunctionMaster> FunctionMasters => Set<FunctionMaster>();
    public DbSet<FunctionGroup> FunctionGroups => Set<FunctionGroup>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<Mode> Modes => Set<Mode>();
    public DbSet<Source> Sources => Set<Source>();
    public DbSet<SkillGroup> SkillGroups => Set<SkillGroup>();
    public DbSet<TrainingGroup> TrainingGroups => Set<TrainingGroup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var aggregates = ChangeTracker.Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ForEach(a => a.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var evt in domainEvents)
            await mediator.Publish(evt, cancellationToken);

        return result;
    }
}
