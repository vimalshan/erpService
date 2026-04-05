using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProxyModule.Domain.Entities;

namespace ProxyModule.Infrastructure.Persistence;

public class ProxyModuleDbContext : DbContext
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProxyModuleDbContext> _logger;

    public ProxyModuleDbContext(DbContextOptions<ProxyModuleDbContext> options, IMediator mediator, ILogger<ProxyModuleDbContext> logger)
        : base(options)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public DbSet<ProxyRight> ProxyRights => Set<ProxyRight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProxyModuleDbContext).Assembly);
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

        foreach (var domainEvent in domainEvents)
        {
            try
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to publish domain event {EventType}. The entity was saved successfully.", domainEvent.GetType().Name);
            }
        }

        return result;
    }
}
