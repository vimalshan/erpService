using AuditLogService.Domain.Entities;
using AuditLogService.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuditLogService.Infrastructure.Persistence;

public class AuditLogDbContext : DbContext
{
    private readonly IMediator _mediator;

    public DbSet<AuditLogEntry> AuditLogs => Set<AuditLogEntry>();

    public AuditLogDbContext(DbContextOptions<AuditLogDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuditLogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity<long>>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entry in ChangeTracker.Entries<Entity<long>>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
