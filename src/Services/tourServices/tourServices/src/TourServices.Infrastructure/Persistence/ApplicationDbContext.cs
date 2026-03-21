using Microsoft.EntityFrameworkCore;
using TourServices.Domain.Aggregates;
using TourServices.Domain.Entities;

namespace TourServices.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TourPackage> TourPackages => Set<TourPackage>();
    public DbSet<TourRegistration> TourRegistrations => Set<TourRegistration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
