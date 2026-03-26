using MediatR;
using Microsoft.EntityFrameworkCore;
using ReferenceDataService.Domain.Common;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Infrastructure.Persistence;

public class ReferenceDataDbContext : DbContext
{
    private readonly IMediator _mediator;

    public DbSet<LovMaster> LovMasters => Set<LovMaster>();
    public DbSet<LovTypeMaster> LovTypeMasters => Set<LovTypeMaster>();
    public DbSet<PathToSqlServer> PathToSqlServers => Set<PathToSqlServer>();

    public ReferenceDataDbContext(DbContextOptions<ReferenceDataDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReferenceDataDbContext).Assembly);
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
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
