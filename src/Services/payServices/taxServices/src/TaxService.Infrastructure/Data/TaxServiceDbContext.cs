using Microsoft.EntityFrameworkCore;
using TaxService.Domain.Common;
using TaxService.Domain.Entities;
using TaxService.Domain.ValueObjects;

namespace TaxService.Infrastructure.Data;

public class TaxServiceDbContext : DbContext
{
    public TaxServiceDbContext(DbContextOptions<TaxServiceDbContext> options) : base(options)
    {
    }

    public DbSet<TaxMarginalDetail> TaxMarginalDetails { get; set; } = null!;
    public DbSet<ConditionalMaster> ConditionalMasters { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore domain events
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.Ignore<TaxMarginalDetailCreatedEvent>();
        modelBuilder.Ignore<TaxCalculatedEvent>();
        modelBuilder.Ignore<ConditionalMasterCreatedEvent>();
        modelBuilder.Ignore<ConditionalMasterDeactivatedEvent>();
        
        // Ignore value objects that aren't directly mapped
        modelBuilder.Ignore<TaxRate>();

        // Configure TaxMarginalDetail entity
        modelBuilder.Entity<TaxMarginalDetail>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.EmployeeSystemId)
                .IsRequired();

            entity.Property(e => e.FinancialYear)
                .IsRequired();

            // Configure Money value object
            entity.OwnsOne(e => e.GrossIncome, nav =>
            {
                nav.Property(m => m.Amount)
                    .HasColumnName("GrossIncome")
                    .HasPrecision(19, 2);
                nav.Property(m => m.Currency)
                    .HasColumnName("GrossIncomeCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            entity.OwnsOne(e => e.StandardDeduction, nav =>
            {
                nav.Property(m => m.Amount)
                    .HasColumnName("StandardDeduction")
                    .HasPrecision(19, 2);
                nav.Property(m => m.Currency)
                    .HasColumnName("StandardDeductionCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            entity.OwnsOne(e => e.TaxableIncome, nav =>
            {
                nav.Property(m => m.Amount)
                    .HasColumnName("TaxableIncome")
                    .HasPrecision(19, 2);
                nav.Property(m => m.Currency)
                    .HasColumnName("TaxableIncomeCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            entity.OwnsOne(e => e.CalculatedTax, nav =>
            {
                nav.Property(m => m.Amount)
                    .HasColumnName("CalculatedTax")
                    .HasPrecision(19, 2);
                nav.Property(m => m.Currency)
                    .HasColumnName("CalculatedTaxCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            entity.Property(e => e.Exemptions)
                .HasMaxLength(500);

            entity.Property(e => e.Remarks)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            // Index for common queries
            entity.HasIndex(e => new { e.EmployeeSystemId, e.FinancialYear })
                .IsUnique(false)
                .HasDatabaseName("IX_EmployeeSystemId_FinancialYear");

            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("IX_CreatedAt");
        });

        // Configure ConditionalMaster entity
        modelBuilder.Entity<ConditionalMaster>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.PayeeId)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.PayeeName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.PayeeAddress)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.PayeePAN)
                .HasMaxLength(10);

            entity.Property(e => e.TaxRegime)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("Old");

            entity.Property(e => e.FinancialYear)
                .IsRequired();

            // Configure Money value objects
            entity.OwnsOne(e => e.TotalExemption, nav =>
            {
                nav.Property(m => m.Amount)
                    .HasColumnName("TotalExemption")
                    .HasPrecision(19, 2);
                nav.Property(m => m.Currency)
                    .HasColumnName("TotalExemptionCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            entity.OwnsOne(e => e.TotalDeduction, nav =>
            {
                nav.Property(m => m.Amount)
                    .HasColumnName("TotalDeduction")
                    .HasPrecision(19, 2);
                nav.Property(m => m.Currency)
                    .HasColumnName("TotalDeductionCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            // Configure owned entities
            entity.OwnsMany(e => e.Exemptions, nav =>
            {
                nav.HasKey(e => e.Id);
                nav.Property(e => e.Code).HasMaxLength(50).IsRequired();
                nav.Property(e => e.Description).HasMaxLength(500);

                nav.OwnsOne(e => e.Amount, m =>
                {
                    m.Property(mo => mo.Amount)
                        .HasColumnName("ExemptionAmount")
                        .HasPrecision(19, 2);
                    m.Property(mo => mo.Currency)
                        .HasColumnName("ExemptionCurrency")
                        .HasMaxLength(3)
                        .HasDefaultValue("INR");
                });
            });

            entity.OwnsMany(e => e.Deductions, nav =>
            {
                nav.HasKey(e => e.Id);
                nav.Property(e => e.Code).HasMaxLength(50).IsRequired();
                nav.Property(e => e.Description).HasMaxLength(500);

                nav.OwnsOne(e => e.Amount, m =>
                {
                    m.Property(mo => mo.Amount)
                        .HasColumnName("DeductionAmount")
                        .HasPrecision(19, 2);
                    m.Property(mo => mo.Currency)
                        .HasColumnName("DeductionCurrency")
                        .HasMaxLength(3)
                        .HasDefaultValue("INR");
                });
            });

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            // Indexes
            entity.HasIndex(e => new { e.PayeeId, e.FinancialYear })
                .IsUnique(false)
                .HasDatabaseName("IX_PayeeId_FinancialYear");

            entity.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_IsActive");

            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("IX_CreatedAt");
        });
    }
}
