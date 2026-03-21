using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Common;
using MediatR;

namespace BookingService.Infrastructure.Persistence;

public class BookingDbContext(DbContextOptions<BookingDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<BookRequestMain> BookRequestMains => Set<BookRequestMain>();
    public DbSet<BookRequestTicket> BookRequestTickets => Set<BookRequestTicket>();
    public DbSet<BookRequestStay> BookRequestStays => Set<BookRequestStay>();
    public DbSet<BookRequestCab> BookRequestCabs => Set<BookRequestCab>();
    public DbSet<BookRequestCostCentre> BookRequestCostCentres => Set<BookRequestCostCentre>();
    public DbSet<BookRequestOther> BookRequestOthers => Set<BookRequestOther>();
    public DbSet<BookRequestConfirmation> BookRequestConfirmations => Set<BookRequestConfirmation>();
    public DbSet<BookConfirmationCab> BookConfirmationCabs => Set<BookConfirmationCab>();
    public DbSet<BookConfirmationTicket> BookConfirmationTickets => Set<BookConfirmationTicket>();
    public DbSet<BookConfirmationStay> BookConfirmationStays => Set<BookConfirmationStay>();
    public DbSet<BookConfirmationCostCentre> BookConfirmationCostCentres => Set<BookConfirmationCostCentre>();
    public DbSet<BookConfirmationMain> BookConfirmationMains => Set<BookConfirmationMain>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, ct);

        return result;
    }
}
