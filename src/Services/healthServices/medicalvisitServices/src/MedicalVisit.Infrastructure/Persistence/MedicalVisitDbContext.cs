using Microsoft.EntityFrameworkCore;
using MedicalVisit.Domain.Entities;
using System.Reflection;

namespace MedicalVisit.Infrastructure.Persistence;

public class MedicalVisitDbContext : DbContext
{
    public MedicalVisitDbContext(DbContextOptions<MedicalVisitDbContext> options)
        : base(options)
    {
    }

    public DbSet<VisitMainAggregate> VisitMains { get; set; } = null!;
    public DbSet<VisitSubRecord> VisitSubRecords { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
