using GroupIncentiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupIncentiveService.Infrastructure.Persistence.Configurations;

public class GroupMasterConfiguration : IEntityTypeConfiguration<GroupMaster>
{
    public void Configure(EntityTypeBuilder<GroupMaster> builder)
    {
        builder.ToTable("Group_Master");
        builder.HasKey(e => e.GroupId);
        builder.Property(e => e.GroupId).HasColumnName("GROUP_ID").ValueGeneratedNever();
        builder.Property(e => e.GroupName).HasColumnName("GROUP_NAME").HasMaxLength(255).IsRequired();
        builder.Property(e => e.GroupDescription).HasColumnName("GROUP_DESCRIPTION").HasMaxLength(500);
        builder.Property(e => e.GroupEffDate).HasColumnName("GROUP_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.GroupClsDate).HasColumnName("GROUP_CLSDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.GroupStatus).HasColumnName("GROUP_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(e => e.GroupLastModifiedBy).HasColumnName("GROUP_LASTMODIFIEDBY");
        builder.Property(e => e.GroupLastModifiedOn).HasColumnName("GROUP_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.GroupName).IsUnique().HasDatabaseName("UQ_GROUP_NAME");
        builder.HasIndex(e => e.GroupStatus).HasDatabaseName("IX_Group_Master_STATUS");

        builder.HasMany(e => e.EmployeeMappings).WithOne(m => m.Group)
            .HasForeignKey(m => m.GrpEmpMapGroupId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.IncentiveBreaks).WithOne(b => b.Group)
            .HasForeignKey(b => b.GrpIncBrkGroupId).OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class GroupEmployeeMapConfiguration : IEntityTypeConfiguration<GroupEmployeeMap>
{
    public void Configure(EntityTypeBuilder<GroupEmployeeMap> builder)
    {
        builder.ToTable("GROUP_EMPLOYEEMAP");
        builder.HasKey(e => e.GrpEmpMapId);
        builder.Property(e => e.GrpEmpMapId).HasColumnName("GRPEMPMAP_ID").ValueGeneratedNever();
        builder.Property(e => e.GrpEmpMapGroupId).HasColumnName("GRPEMPMAP_GROUPID");
        builder.Property(e => e.GrpEmpMapEmpSysId).HasColumnName("GRPEMPMAP_EMPSYSID");
        builder.Property(e => e.GrpEmpMapEffDate).HasColumnName("GRPEMPMAP_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.GrpEmpMapClsDate).HasColumnName("GRPEMPMAP_CLSDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.GrpEmpMapRole).HasColumnName("GRPEMPMAP_ROLE").HasMaxLength(50);
        builder.Property(e => e.GrpEmpMapLastModifiedBy).HasColumnName("GRPEMPMAP_LASTMODIFIEDBY");
        builder.Property(e => e.GrpEmpMapLastModifiedOn).HasColumnName("GRPEMPMAP_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.GrpEmpMapGroupId).HasDatabaseName("IX_GROUP_EMPLOYEEMAP_GROUPID");
        builder.HasIndex(e => e.GrpEmpMapEmpSysId).HasDatabaseName("IX_GROUP_EMPLOYEEMAP_EMPSYSID");
        builder.HasIndex(e => new { e.GrpEmpMapGroupId, e.GrpEmpMapEmpSysId, e.GrpEmpMapEffDate })
            .IsUnique().HasDatabaseName("UQ_GRPEMPMAP");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class GroupIncentiveMainConfiguration : IEntityTypeConfiguration<GroupIncentiveMain>
{
    public void Configure(EntityTypeBuilder<GroupIncentiveMain> builder)
    {
        builder.ToTable("GROUPINCENTIVE_MAIN");
        builder.HasKey(e => e.GrpIncId);
        builder.Property(e => e.GrpIncId).HasColumnName("GRPINC_ID").ValueGeneratedNever();
        builder.Property(e => e.GrpIncGroupId).HasColumnName("GRPINC_GROUPID");
        builder.Property(e => e.GrpIncIncMonth).HasColumnName("GRPINC_INCMONTH");
        builder.Property(e => e.GrpIncIncYear).HasColumnName("GRPINC_INCYEAR");
        builder.Property(e => e.GrpIncTotalAmount).HasColumnName("GRPINC_TOTALAMOUNT").HasColumnType("decimal(15,2)");
        builder.Property(e => e.GrpIncAppStatus).HasColumnName("GRPINC_APPSTATUS").HasMaxLength(1).IsRequired();
        builder.Property(e => e.GrpIncApprovedAmount).HasColumnName("GRPINC_APPROVEDAMOUNT").HasColumnType("decimal(15,2)");
        builder.Property(e => e.GrpIncApprover).HasColumnName("GRPINC_APPROVER");
        builder.Property(e => e.GrpIncApprovalDate).HasColumnName("GRPINC_APPROVALDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.GrpIncEnteredOn).HasColumnName("GRPINC_ENTEREDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.GrpIncEnteredBy).HasColumnName("GRPINC_ENTEREDBY");
        builder.Property(e => e.GrpIncLastModifiedBy).HasColumnName("GRPINC_LASTMODIFIEDBY");
        builder.Property(e => e.GrpIncLastModifiedOn).HasColumnName("GRPINC_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.GrpIncGroupId).HasDatabaseName("IX_GROUPINCENTIVE_MAIN_GROUPID");
        builder.HasIndex(e => e.GrpIncAppStatus).HasDatabaseName("IX_GROUPINCENTIVE_MAIN_APPSTATUS");

        builder.HasOne(e => e.Group)
            .WithMany()
            .HasForeignKey(e => e.GrpIncGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Details).WithOne(d => d.Main)
            .HasForeignKey(d => d.GrpIncDetMainId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Approvals).WithOne(a => a.Main)
            .HasForeignKey(a => a.GrpIncAppMainId).OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class GroupIncentiveDetConfiguration : IEntityTypeConfiguration<GroupIncentiveDet>
{
    public void Configure(EntityTypeBuilder<GroupIncentiveDet> builder)
    {
        builder.ToTable("GROUPINCENTIVE_DET");
        builder.HasKey(e => e.GrpIncDetId);
        builder.Property(e => e.GrpIncDetId).HasColumnName("GRPINCDET_ID").ValueGeneratedNever();
        builder.Property(e => e.GrpIncDetMainId).HasColumnName("GRPINCDET_MAINID");
        builder.Property(e => e.GrpIncDetEmpSysId).HasColumnName("GRPINCDET_EMPSYSID");
        builder.Property(e => e.GrpIncDetAllocPercentage).HasColumnName("GRPINCDET_ALLOCPERCENTAGE").HasColumnType("decimal(5,2)");
        builder.Property(e => e.GrpIncDetAllocAmount).HasColumnName("GRPINCDET_ALLOCAMOUNT").HasColumnType("decimal(15,2)");
        builder.Property(e => e.GrpIncDetApprovedAmount).HasColumnName("GRPINCDET_APPROVEDAMOUNT").HasColumnType("decimal(15,2)");
        builder.Property(e => e.GrpIncDetAppStatus).HasColumnName("GRPINCDET_APPSTATUS").HasMaxLength(1).IsRequired();
        builder.Property(e => e.GrpIncDetLastModifiedBy).HasColumnName("GRPINCDET_LASTMODIFIEDBY");
        builder.Property(e => e.GrpIncDetLastModifiedOn).HasColumnName("GRPINCDET_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.GrpIncDetMainId).HasDatabaseName("IX_GROUPINCENTIVE_DET_MAINID");
        builder.HasIndex(e => e.GrpIncDetAppStatus).HasDatabaseName("IX_GROUPINCENTIVE_DET_APPSTATUS");
        builder.HasIndex(e => new { e.GrpIncDetMainId, e.GrpIncDetEmpSysId })
            .IsUnique().HasDatabaseName("UQ_GRPINCDET");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class GroupIncentiveBreakConfiguration : IEntityTypeConfiguration<GroupIncentiveBreak>
{
    public void Configure(EntityTypeBuilder<GroupIncentiveBreak> builder)
    {
        builder.ToTable("GROUPINCENTIVE_BREAK");
        builder.HasKey(e => e.GrpIncBrkId);
        builder.Property(e => e.GrpIncBrkId).HasColumnName("GRPINCBRK_ID").ValueGeneratedNever();
        builder.Property(e => e.GrpIncBrkGroupId).HasColumnName("GRPINCBRK_GROUPID");
        builder.Property(e => e.GrpIncBrkAttPercentage).HasColumnName("GRPINCBRK_ATTPERCENTAGE").HasColumnType("decimal(5,2)");
        builder.Property(e => e.GrpIncBrkIncPercentage).HasColumnName("GRPINCBRK_INCPERCENTAGE").HasColumnType("decimal(5,2)");
        builder.Property(e => e.GrpIncBrkEffDate).HasColumnName("GRPINCBRK_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.GrpIncBrkClsDate).HasColumnName("GRPINCBRK_CLSDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.GrpIncBrkLastModifiedBy).HasColumnName("GRPINCBRK_LASTMODIFIEDBY");
        builder.Property(e => e.GrpIncBrkLastModifiedOn).HasColumnName("GRPINCBRK_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class GroupIncentiveApprovalConfiguration : IEntityTypeConfiguration<GroupIncentiveApproval>
{
    public void Configure(EntityTypeBuilder<GroupIncentiveApproval> builder)
    {
        builder.ToTable("GROUPINCENTIVE_APPROVAL");
        builder.HasKey(e => e.GrpIncAppId);
        builder.Property(e => e.GrpIncAppId).HasColumnName("GRPINCAPP_ID").ValueGeneratedNever();
        builder.Property(e => e.GrpIncAppMainId).HasColumnName("GRPINCAPP_MAINID");
        builder.Property(e => e.GrpIncAppApprover).HasColumnName("GRPINCAPP_APPROVER");
        builder.Property(e => e.GrpIncAppStatus).HasColumnName("GRPINCAPP_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(e => e.GrpIncAppRemarks).HasColumnName("GRPINCAPP_REMARKS").HasMaxLength(500);
        builder.Property(e => e.GrpIncAppApprovalDate).HasColumnName("GRPINCAPP_APPROVALDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.GrpIncAppLastModifiedBy).HasColumnName("GRPINCAPP_LASTMODIFIEDBY");
        builder.Property(e => e.GrpIncAppLastModifiedOn).HasColumnName("GRPINCAPP_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.Ignore(e => e.DomainEvents);
    }
}
