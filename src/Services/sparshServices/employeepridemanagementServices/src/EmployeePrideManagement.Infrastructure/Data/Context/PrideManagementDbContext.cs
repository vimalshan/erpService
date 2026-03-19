using EmployeePrideManagement.Domain.Entities;
using EmployeePrideManagement.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeePrideManagement.Infrastructure.Data.Context;

public class PrideManagementDbContext : DbContext
{
    private readonly IMediator _mediator;

    public DbSet<MomentPride> MomentPrides => Set<MomentPride>();

    public PrideManagementDbContext(DbContextOptions<PrideManagementDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PrideManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
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
