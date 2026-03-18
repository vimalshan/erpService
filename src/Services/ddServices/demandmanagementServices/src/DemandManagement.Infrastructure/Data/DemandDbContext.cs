using DemandManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemandManagement.Infrastructure.Data;

public class DemandDbContext : DbContext
{
    public DemandDbContext(DbContextOptions<DemandDbContext> options) : base(options) { }

    public DbSet<DemandMaster> DemandMaster { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DemandMaster>(entity =>
        {
            entity.HasKey(e => e.DemandId);
            entity.ToTable("DEMAND_MASTER");

            entity.Property(e => e.DemandId).HasColumnName("DEMAND_ID");
            entity.Property(e => e.DemandType).HasColumnName("DEMAND_TYPE").HasMaxLength(50);
            entity.Property(e => e.DepartmentId).HasColumnName("DEPARTMENT_ID");
            entity.Property(e => e.DemandDescription).HasColumnName("DEMAND_DESCRIPTION").HasMaxLength(500);
            entity.Property(e => e.RequiredDate).HasColumnName("REQUIRED_DATE");
            entity.Property(e => e.Priority).HasColumnName("PRIORITY").HasMaxLength(10);
            entity.Property(e => e.DemandStatus).HasColumnName("DEMAND_STATUS").HasMaxLength(1);
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
            entity.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");
            entity.Property(e => e.ApprovalRemarks).HasColumnName("APPROVAL_REMARKS").HasMaxLength(500);
            entity.Property(e => e.ApprovedBy).HasColumnName("APPROVED_BY");
            entity.Property(e => e.ApprovalDate).HasColumnName("APPROVAL_DATE");
            entity.Property(e => e.CompletionRemarks).HasColumnName("COMPLETION_REMARKS").HasMaxLength(500);
            entity.Property(e => e.CompletedBy).HasColumnName("COMPLETED_BY");
            entity.Property(e => e.CompletionDate).HasColumnName("COMPLETION_DATE");
        });
    }
}
