using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookingService.Infrastructure.Data;

public class BookingDbContext : DbContext, IUnitOfWork
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

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
        => await base.SaveChangesAsync(ct);
}
