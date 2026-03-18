using MediatR;
using Microsoft.EntityFrameworkCore;
using TrustService.Application.Common.Interfaces;
using TrustService.Domain.Common;
using TrustService.Domain.Entities;

namespace TrustService.Infrastructure.Persistence;

public class TrustDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public TrustDbContext(DbContextOptions<TrustDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<TrustMaster> TrustMasters => Set<TrustMaster>();
    public DbSet<TrustFundType> TrustFundTypes => Set<TrustFundType>();
    public DbSet<TrustRole> TrustRoles => Set<TrustRole>();
    public DbSet<TrustApprover> TrustApprovers => Set<TrustApprover>();
    public DbSet<TrustConfiguration> TrustConfigurations => Set<TrustConfiguration>();
    public DbSet<TrustAuditLog> TrustAuditLogs => Set<TrustAuditLog>();
    public DbSet<TrustUnit> TrustUnits => Set<TrustUnit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrustDbContext).Assembly);
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
