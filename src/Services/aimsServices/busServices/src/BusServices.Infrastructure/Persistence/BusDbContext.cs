using BusServices.Domain.Entities;
using BusServices.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BusServices.Infrastructure.Persistence;

public sealed class BusDbContext : DbContext
{
    public BusDbContext(DbContextOptions<BusDbContext> options) : base(options) { }

    public DbSet<Bus> Buses => Set<Bus>();
    public DbSet<BusRoute> BusRoutes => Set<BusRoute>();
    public DbSet<EmployeeBus> EmployeeBusAssignments => Set<EmployeeBus>();
    public DbSet<BusArrival> BusArrivals => Set<BusArrival>();
    public DbSet<BusDeductionRate> BusDeductionRates => Set<BusDeductionRate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BusDbContext).Assembly);
    }
}
