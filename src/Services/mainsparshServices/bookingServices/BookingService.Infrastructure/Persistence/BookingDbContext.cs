using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookingService.Infrastructure.Persistence;

public class BookingDbContext(DbContextOptions<BookingDbContext> options) : DbContext(options)
{
    public DbSet<BookMain> BookMains => Set<BookMain>();
    public DbSet<BookRecord> BookRecords => Set<BookRecord>();
    public DbSet<BookAttendee> BookAttendees => Set<BookAttendee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
