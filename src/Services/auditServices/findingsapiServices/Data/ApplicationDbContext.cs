// Data/ApplicationDbContext.cs
using FindingsAPI.Gateway;
using Microsoft.EntityFrameworkCore;

namespace FindingsAPI.Gateway.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Finding> Findings { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Site> Sites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Finding>(e =>
            {
                e.ToTable("Findings");
                e.HasKey(f => f.FindingId);
                // CompanyId is not a real DB column (derived from JOIN) — ignore it for writes
                e.Ignore(f => f.CompanyId);
                // Status/Category are from JOIN — ignore for writes
                e.Ignore(f => f.Status);
                e.Ignore(f => f.Category);
                e.Ignore(f => f.Response);
                e.Ignore(f => f.ClosureNotes);
                e.Ignore(f => f.ClosedBy);
                e.Ignore(f => f.OpenDate);
                e.Ignore(f => f.Services);
                e.Ignore(f => f.Company);
                e.Ignore(f => f.Site);
            });

            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<Site>().ToTable("Sites");
        }
    }
}