using Microsoft.EntityFrameworkCore;
using ScholarshipService.Domain.Entities;

namespace ScholarshipService.Infrastructure.Data;

public class ScholarshipDbContext : DbContext
{
    public ScholarshipDbContext(DbContextOptions<ScholarshipDbContext> options) : base(options) { }

    public DbSet<ScholarshipMain> ScholarshipMains => Set<ScholarshipMain>();
    public DbSet<ScholarshipDetail> ScholarshipDetails => Set<ScholarshipDetail>();
    public DbSet<ScholarshipAmount> ScholarshipAmounts => Set<ScholarshipAmount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScholarshipDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
