using MediatR;
using MenuAndSecurityService.Domain.Common;
using MenuAndSecurityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MenuAndSecurityService.Infrastructure.Persistence;

public class MenuSecurityDbContext : DbContext
{
    private readonly IMediator _mediator;

    public DbSet<MenuMaster> MenuMasters => Set<MenuMaster>();
    public DbSet<RoleMenuAccess> RoleMenuAccesses => Set<RoleMenuAccess>();

    public MenuSecurityDbContext(DbContextOptions<MenuSecurityDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MenuSecurityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
