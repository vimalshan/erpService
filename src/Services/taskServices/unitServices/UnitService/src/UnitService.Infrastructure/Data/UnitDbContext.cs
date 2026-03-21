using MediatR;
using Microsoft.EntityFrameworkCore;
using UnitService.Domain.Entities;

namespace UnitService.Infrastructure.Data;

public class UnitDbContext : DbContext
{
    private readonly IMediator _mediator;

    public UnitDbContext(DbContextOptions<UnitDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<EquipmentMaster> EquipmentMasters => Set<EquipmentMaster>();
    public DbSet<CategoryMaster> CategoryMasters => Set<CategoryMaster>();
    public DbSet<EquipmentStatus> EquipmentStatuses => Set<EquipmentStatus>();
    public DbSet<AccessMaster> AccessMasters => Set<AccessMaster>();
    public DbSet<BudgetMaster> BudgetMasters => Set<BudgetMaster>();
    public DbSet<MailIdMaster> MailIdMasters => Set<MailIdMaster>();
    public DbSet<StatusConfirm> StatusConfirms => Set<StatusConfirm>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<UnitService.Domain.Events.DomainEvent>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnitDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
