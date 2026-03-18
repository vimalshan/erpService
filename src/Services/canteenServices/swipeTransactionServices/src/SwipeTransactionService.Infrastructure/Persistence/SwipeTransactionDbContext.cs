using Microsoft.EntityFrameworkCore;
using SwipeTransactionService.Domain.Entities;

namespace SwipeTransactionService.Infrastructure.Persistence;

public sealed class SwipeTransactionDbContext : DbContext
{
    public SwipeTransactionDbContext(DbContextOptions<SwipeTransactionDbContext> options)
        : base(options) { }

    public DbSet<SwipeCardUpload> SwipeCardUploads => Set<SwipeCardUpload>();
    public DbSet<CanteenPunch> CanteenPunches => Set<CanteenPunch>();
    public DbSet<DailyAvailed> DailyAvaileds => Set<DailyAvailed>();
    public DbSet<CanteenDacon> CanteenDacons => Set<CanteenDacon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SwipeTransactionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
