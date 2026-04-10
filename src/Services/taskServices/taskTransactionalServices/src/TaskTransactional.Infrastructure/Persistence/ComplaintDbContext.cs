using TaskTransactional.Domain.Common;
using TaskTransactional.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaskTransactional.Infrastructure.Persistence;

public class ComplaintDbContext(DbContextOptions<ComplaintDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<ComplaintMain> ComplaintMains => Set<ComplaintMain>();
    public DbSet<ComplaintDetail> ComplaintDetails => Set<ComplaintDetail>();
    public DbSet<ComplaintTask> ComplaintTasks => Set<ComplaintTask>();
    public DbSet<ComplaintAction> ComplaintActions => Set<ComplaintAction>();
    public DbSet<ComplaintHistory> ComplaintHistories => Set<ComplaintHistory>();
    public DbSet<ComplaintEscalation> ComplaintEscalations => Set<ComplaintEscalation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ComplaintDbContext).Assembly);
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
