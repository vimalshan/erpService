using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrustService.Application.Common.Interfaces;
using TrustService.Domain.Common;
using TrustService.Domain.Entities;

namespace TrustService.Infrastructure.Persistence;

public class TrustDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    private readonly ILogger<TrustDbContext> _logger;

    public TrustDbContext(DbContextOptions<TrustDbContext> options, IMediator mediator, ILogger<TrustDbContext> logger)
        : base(options)
    {
        _mediator = mediator;
        _logger = logger;
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
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // Dispatch domain events after saving — failures must not bubble up
        // since the DB transaction is already committed
        foreach (var domainEvent in domainEvents)
        {
            try
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to dispatch domain event {EventType}", domainEvent.GetType().Name);
            }
        }

        return result;
    }
}
