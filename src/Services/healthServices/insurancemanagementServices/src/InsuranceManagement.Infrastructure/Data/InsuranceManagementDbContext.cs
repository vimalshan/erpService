using Microsoft.EntityFrameworkCore;
using InsuranceManagement.Domain.Entities;

namespace InsuranceManagement.Infrastructure.Data;

/// <summary>
/// EF Core DbContext for Insurance Management
/// </summary>
public class InsuranceManagementDbContext : DbContext
{
    public DbSet<InsurancePlan> InsurancePlans { get; set; }
    public DbSet<InsuranceEnrollment> InsuranceEnrollments { get; set; }
    public DbSet<InsuranceClaim> InsuranceClaims { get; set; }

    public InsuranceManagementDbContext(DbContextOptions<InsuranceManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore DomainEvent to prevent EF Core from trying to map it
        modelBuilder.Ignore<InsuranceManagement.Domain.Common.DomainEvent>();

        // Configure InsurancePlan
        modelBuilder.Entity<InsurancePlan>(entity =>
        {
            entity.HasKey(e => e.InsurancePlanId);
            entity.Property(e => e.PlanName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PlanDescription).HasMaxLength(500);
            entity.Property(e => e.PremiumRate).HasPrecision(5, 2);
            entity.Property(e => e.MinPremium).HasPrecision(19, 0);
            entity.Property(e => e.MaxPremium).HasPrecision(19, 0);
            entity.Property(e => e.CoverageDetails).HasMaxLength(1000);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => new { e.IsActive, e.CreatedOn });
        });

        // Configure InsuranceEnrollment
        modelBuilder.Entity<InsuranceEnrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId);
            entity.Property(e => e.EmpSysId).IsRequired();
            entity.Property(e => e.InsurancePlanId).IsRequired();
            
            // Configure CoverageType as owned type
            entity.OwnsOne(e => e.CoverageType, nav =>
            {
                nav.Property(ct => ct.Value).HasColumnName("COVERAGE_TYPE").HasMaxLength(20);
            });

            // Configure EnrollmentStatus as owned type
            entity.OwnsOne(e => e.Status, nav =>
            {
                nav.Property(s => s.Value).HasColumnName("ENROLLMENT_STATUS").HasMaxLength(1);
                nav.HasIndex(s => s.Value);
            });

            entity.Property(e => e.MonthlyPremium).HasPrecision(19, 0);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            
            // Relationships
            entity.HasOne(e => e.InsurancePlan)
                .WithMany()
                .HasForeignKey(e => e.InsurancePlanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Claims)
                .WithOne(c => c.Enrollment)
                .HasForeignKey(c => c.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.EmpSysId);
            entity.HasIndex(e => e.InsurancePlanId);
        });

        // Configure InsuranceClaim
        modelBuilder.Entity<InsuranceClaim>(entity =>
        {
            entity.HasKey(e => e.ClaimId);
            entity.Property(e => e.EmpSysId).IsRequired();
            entity.Property(e => e.EnrollmentId).IsRequired();
            entity.Property(e => e.InsurancePlanId).IsRequired();

            // Configure ClaimType as owned type
            entity.OwnsOne(e => e.ClaimType, nav =>
            {
                nav.Property(ct => ct.Value).HasColumnName("CLAIM_TYPE").HasMaxLength(20);
            });

            // Configure Money value objects
            entity.OwnsOne(e => e.ClaimAmount, nav =>
            {
                nav.Property(m => m.Amount).HasColumnName("CLAIM_AMOUNT").HasPrecision(19, 0);
            });

            entity.OwnsOne(e => e.ReimbursableAmount, nav =>
            {
                nav.Property(m => m.Amount).HasColumnName("REIMBURSABLE_AMOUNT").HasPrecision(19, 0);
            });

            entity.OwnsOne(e => e.ApprovedAmount, nav =>
            {
                nav.Property(m => m.Amount).HasColumnName("APPROVED_AMOUNT").HasPrecision(19, 0);
            });

            // Configure ClaimStatus as owned type
            entity.OwnsOne(e => e.Status, nav =>
            {
                nav.Property(s => s.Value).HasColumnName("CLAIM_STATUS").HasMaxLength(10);
                nav.HasIndex(s => s.Value);
            });

            entity.Property(e => e.HospitalName).HasMaxLength(100);
            entity.Property(e => e.ClaimRemarks).HasMaxLength(500);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            entity.HasOne(e => e.Enrollment)
                .WithMany(en => en.Claims)
                .HasForeignKey(e => e.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.EmpSysId);
            entity.HasIndex(e => e.EnrollmentId);
            entity.HasIndex(e => e.CreatedOn);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
