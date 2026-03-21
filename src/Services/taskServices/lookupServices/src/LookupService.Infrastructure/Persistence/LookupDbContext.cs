using LookupService.Domain.Common;
using LookupService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LookupService.Infrastructure.Persistence;

public class LookupDbContext(DbContextOptions<LookupDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<LovTypeMaster> LovTypeMasters => Set<LovTypeMaster>();
    public DbSet<LovMaster> LovMasters => Set<LovMaster>();
    public DbSet<LovUnitMap> LovUnitMaps => Set<LovUnitMap>();
    public DbSet<LovPanelMap> LovPanelMaps => Set<LovPanelMap>();
    public DbSet<PanelMaster> PanelMasters => Set<PanelMaster>();
    public DbSet<ProcessMaster> ProcessMasters => Set<ProcessMaster>();
    public DbSet<UnitProcessMap> UnitProcessMaps => Set<UnitProcessMap>();
    public DbSet<UnitLovAccessMaster> UnitLovAccessMasters => Set<UnitLovAccessMaster>();
    public DbSet<UnitLovAccessDetail> UnitLovAccessDetails => Set<UnitLovAccessDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LookupDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents).ToList();
        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
