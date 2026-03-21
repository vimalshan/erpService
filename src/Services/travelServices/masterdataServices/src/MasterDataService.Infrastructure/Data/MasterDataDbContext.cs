using MasterDataService.Domain.Common;
using MasterDataService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MasterDataService.Infrastructure.Data;

public class MasterDataDbContext : DbContext
{
    private readonly IMediator _mediator;

    public MasterDataDbContext(DbContextOptions<MasterDataDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<GuestHouse> GuestHouses => Set<GuestHouse>();
    public DbSet<GuestHouseRoom> GuestHouseRooms => Set<GuestHouseRoom>();
    public DbSet<GuestRoomAvailability> GuestRoomAvailabilities => Set<GuestRoomAvailability>();
    public DbSet<GlCodeCombination> GlCodeCombinations => Set<GlCodeCombination>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<TaxSlab> TaxSlabs => Set<TaxSlab>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Domain.Entities.Route> Routes => Set<Domain.Entities.Route>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterDataDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents).ToList();
        domainEntities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        // Set audit fields
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
