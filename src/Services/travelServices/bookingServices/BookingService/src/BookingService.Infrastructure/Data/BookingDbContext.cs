using BookingService.Application.EventHandlers;
using BookingService.Domain.Common;
using BookingService.Domain.Entities;
using BookingService.Domain.Events;
using BookingService.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookingService.Infrastructure.Data;

public class BookingDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;
    private readonly List<IDomainEvent> _pendingDomainEvents = new();

    public BookingDbContext(DbContextOptions<BookingDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<BookingRequest> BookingRequests => Set<BookingRequest>();
    public DbSet<BookingConfirmation> BookingConfirmations => Set<BookingConfirmation>();
    public DbSet<BookingForwardUnit> BookingForwardUnits => Set<BookingForwardUnit>();
    public DbSet<CouponRequest> CouponRequests => Set<CouponRequest>();
    public DbSet<CouponMain> CouponMains => Set<CouponMain>();
    public DbSet<CouponSub> CouponSubs => Set<CouponSub>();
    public DbSet<CabPick> CabPicks => Set<CabPick>();
    public DbSet<RoomAvailTemp> RoomAvailTemps => Set<RoomAvailTemp>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEvents = _pendingDomainEvents.ToList();
        _pendingDomainEvents.Clear();

        var result = await base.SaveChangesAsync(ct);

        // Dispatch domain events as MediatR notifications
        foreach (var domainEvent in domainEvents)
        {
            INotification notification = domainEvent switch
            {
                BookingCreatedEvent e => new BookingCreatedNotification(e),
                BookingConfirmedEvent e => new BookingConfirmedNotification(e),
                BookingCancelledEvent e => new BookingCancelledNotification(e),
                _ => throw new InvalidOperationException($"Unknown domain event: {domainEvent.GetType().Name}")
            };
            await _publisher.Publish(notification, ct);
        }

        return result;
    }

    public void AddDomainEvents(IEnumerable<IDomainEvent> events)
        => _pendingDomainEvents.AddRange(events);
}
