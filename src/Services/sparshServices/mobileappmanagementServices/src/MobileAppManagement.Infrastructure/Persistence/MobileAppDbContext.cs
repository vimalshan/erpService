using MediatR;
using Microsoft.EntityFrameworkCore;
using MobileAppManagement.Domain.Entities;

namespace MobileAppManagement.Infrastructure.Persistence;

public class MobileAppDbContext(DbContextOptions<MobileAppDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public DbSet<AppDeviceDetail> AppDeviceDetails => Set<AppDeviceDetail>();
    public DbSet<LoginDetail> LoginDetails => Set<LoginDetail>();
    public DbSet<AppRegistration> AppRegistrations => Set<AppRegistration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MobileAppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<Domain.Common.BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}
