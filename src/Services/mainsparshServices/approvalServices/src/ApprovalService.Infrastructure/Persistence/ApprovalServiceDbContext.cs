namespace ApprovalService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Common;
using ApprovalService.Domain.ValueObjects;

/// <summary>
/// Database context for Approval Service
/// </summary>
public class ApprovalServiceDbContext : DbContext
{
    public ApprovalServiceDbContext(DbContextOptions<ApprovalServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApprovalMaster> ApprovalMasters { get; set; }
    public DbSet<ApproverEmployee> ApproverEmployees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure ApprovalMaster
        modelBuilder.Entity<ApprovalMaster>(entity =>
        {
            entity.ToTable("APPR_MAST");

            entity.HasKey(e => e.Id).HasName("PK_APPR_MAST");

            entity.Property(e => e.Id)
                .HasColumnName("APPR_ID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Code)
                .HasColumnName("APPR_CODE")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Name)
                .HasColumnName("APPR_NAME")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Module)
                .HasColumnName("APPR_MODULE")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Level)
                .HasColumnName("APPR_LEVEL")
                .HasDefaultValue(1);

            entity.Property(e => e.Status)
                .HasColumnName("APPR_STATUS")
                .HasConversion(
                    v => v == ApprovalStatus.Active ? 'A' : 'I',
                    v => v == 'A' ? ApprovalStatus.Active : ApprovalStatus.Inactive);

            entity.Property(e => e.CreatedBy)
                .HasColumnName("CREATED_BY")
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                .HasColumnName("CREATED_ON")
                .HasDefaultValueSql("GETDATE()")
                .HasColumnType("datetime2(3)");

            entity.Property(e => e.UpdatedBy)
                .HasColumnName("UPDATED_BY");

            entity.Property(e => e.UpdatedOn)
                .HasColumnName("UPDATED_ON")
                .HasColumnType("datetime2(3)");

            entity.HasIndex(e => e.Code)
                .HasName("UQ_APPR_CODE")
                .IsUnique();

            entity.HasIndex(e => e.Module)
                .HasName("IX_APPR_MAST_MODULE");

            entity.Navigation(e => e.Approvers).HasField("_approvers");

            entity.HasMany(e => e.Approvers)
                .WithOne()
                .HasForeignKey(e => e.ApprovalMasterId)
                .HasConstraintName("FK_APPROVER_EMP_APPR_MAST")
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ApproverEmployee
        modelBuilder.Entity<ApproverEmployee>(entity =>
        {
            entity.ToTable("APPROVER_EMP");

            entity.HasKey(e => e.Id).HasName("PK_APPROVER_EMP");

            entity.Property(e => e.Id)
                .HasColumnName("APPROVER_ID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ApprovalMasterId)
                .HasColumnName("APPR_ID")
                .IsRequired();

            entity.Property(e => e.EmployeeSysId)
                .HasColumnName("EMP_SYSID")
                .IsRequired();

            entity.Property(e => e.ApproverLevel)
                .HasColumnName("APPROVER_LEVEL")
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("APPROVER_STATUS")
                .HasConversion(
                    v => v == ApproverStatus.Active ? 'A' : 'I',
                    v => v == 'A' ? ApproverStatus.Active : ApproverStatus.Inactive);

            entity.Property(e => e.EffectiveFrom)
                .HasColumnName("EFFECTIVE_FROM")
                .HasColumnType("date");

            entity.Property(e => e.EffectiveTo)
                .HasColumnName("EFFECTIVE_TO")
                .HasColumnType("date");

            entity.Property(e => e.CreatedBy)
                .HasColumnName("CREATED_BY")
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                .HasColumnName("CREATED_ON")
                .HasDefaultValueSql("GETDATE()")
                .HasColumnType("datetime2(3)");

            entity.Property(e => e.UpdatedBy)
                .HasColumnName("UPDATED_BY");

            entity.Property(e => e.UpdatedOn)
                .HasColumnName("UPDATED_ON")
                .HasColumnType("datetime2(3)");

            entity.HasIndex(e => e.ApprovalMasterId)
                .HasName("IX_APPROVER_EMP_APPR_ID");

            entity.HasIndex(e => e.EmployeeSysId)
                .HasName("IX_APPROVER_EMP_EMP_SYSID");
        });
    }
}
