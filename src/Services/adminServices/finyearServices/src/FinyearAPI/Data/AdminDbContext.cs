using Microsoft.EntityFrameworkCore;
using FinyearAPI.Domain.Entities;

namespace FinyearAPI.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for Financial Year Management
    /// Connection String: Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name="FinyearAPI";Command Timeout=0
    /// Database: ADMINDB
    /// </summary>
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Financial Year Master Database Set
        /// </summary>
        public DbSet<FinancialYearMaster> FinancialYearMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure FINYEAR_MASTER table
            modelBuilder.Entity<FinancialYearMaster>(entity =>
            {
                entity.ToTable("FINYEAR_MASTER");

                entity.HasKey(e => e.FinancialYearId)
                    .HasName("PK_FINYEAR_MASTER");

                entity.Property(e => e.FinancialYearId)
                    .HasColumnName("FY_ID");

                entity.Property(e => e.FinancialYearName)
                    .HasColumnName("FY_NAME")
                    .HasMaxLength(27)
                    .IsRequired();

                entity.Property(e => e.StartDate)
                    .HasColumnName("FY_STARTDATE")
                    .HasColumnType("datetime2(3)");

                entity.Property(e => e.CloseDate)
                    .HasColumnName("FY_CLOSEDATE")
                    .HasColumnType("datetime2(3)");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("FY_UPDATED_BY");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("FY_UPDATED_ON")
                    .HasColumnType("datetime2(3)");

                // Create indexes for performance
                entity.HasIndex(e => e.StartDate)
                    .HasName("IDX_FINYEAR_STARTDATE");
            });
        }
    }
}
