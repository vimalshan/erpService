using AuditService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditService.Infrastructure.Data.Configurations;

public class AuditMasterConfiguration : IEntityTypeConfiguration<AuditMaster>
{
    public void Configure(EntityTypeBuilder<AuditMaster> builder)
    {
        builder.ToTable("AUDIT_MASTER");
        builder.HasKey(x => x.AuditId);
        builder.Property(x => x.AuditId).HasColumnName("AUDIT_ID").ValueGeneratedNever();
        builder.Property(x => x.AuditName).HasColumnName("AUDIT_NAME").HasMaxLength(50).IsRequired();
        builder.Property(x => x.AuditUnit).HasColumnName("AUDIT_UNIT").IsRequired();
        builder.Property(x => x.AuditFrom).HasColumnName("AUDIT_FROM").IsRequired();
        builder.Property(x => x.AuditTo).HasColumnName("AUDIT_TO").IsRequired();
        builder.Property(x => x.AuditDefLocation).HasColumnName("AUDIT_DEFLOCATION").HasMaxLength(50).IsRequired();
        builder.Property(x => x.AuditStatus).HasColumnName("AUDIT_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.AuditCreatedBy).HasColumnName("AUDIT_CREATEDBY").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.AuditCreatedOn).HasColumnName("AUDIT_CREATEDON").IsRequired();
        builder.Property(x => x.AuditUpdatedBy).HasColumnName("AUDIT_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AuditUpdatedOn).HasColumnName("AUDIT_UPDATEDON");
        builder.Property(x => x.AuditPlanYear).HasColumnName("AUDIT_PLANYEAR").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AuditFile1).HasColumnName("AUDIT_FILE1").HasMaxLength(500);
        builder.Property(x => x.AuditFile2).HasColumnName("AUDIT_FILE2").HasMaxLength(500);
        builder.Property(x => x.AuditFile3).HasColumnName("AUDIT_FILE3").HasMaxLength(500);
        builder.Property(x => x.AuditPlanFrom).HasColumnName("AUDIT_PLANFROM").IsRequired();
        builder.Property(x => x.AuditPlanTo).HasColumnName("AUDIT_PLANTO").IsRequired();
        builder.Property(x => x.AuditCompleted).HasColumnName("AUDIT_COMPLETED").HasMaxLength(1);
        builder.Property(x => x.AuditFirmName).HasColumnName("AUDIT_FIRMNAME").HasMaxLength(100);
        builder.Property(x => x.AuditFieldFrom).HasColumnName("AUDIT_FIELDFROM");
        builder.Property(x => x.AuditFieldTo).HasColumnName("AUDIT_FIELDTO");
        builder.Property(x => x.AuditCordId).HasColumnName("AUDIT_CORDID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AuditProcess).HasColumnName("AUDIT_PROCESS");

        builder.HasMany(x => x.Observations)
               .WithOne()
               .HasForeignKey(o => o.ObvAuditId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
    }
}
