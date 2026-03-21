using LookupService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LookupService.Infrastructure.Persistence.Configurations;

public class LovTypeMasterConfiguration : IEntityTypeConfiguration<LovTypeMaster>
{
    public void Configure(EntityTypeBuilder<LovTypeMaster> builder)
    {
        builder.ToTable("LOV_TYPEMASTER");
        builder.HasKey(e => e.LovTypeCode);
        builder.Property(e => e.LovTypeCode).HasColumnName("LOV_TYPECODE").HasColumnType("char(3)").IsRequired();
        builder.Property(e => e.LovTypeName).HasColumnName("LOV_TYPENAME").HasMaxLength(50);
        builder.HasMany(e => e.LovMasters).WithOne(e => e.LovTypeMasterNavigation).HasForeignKey(e => e.LovType);
    }
}

public class LovMasterConfiguration : IEntityTypeConfiguration<LovMaster>
{
    public void Configure(EntityTypeBuilder<LovMaster> builder)
    {
        builder.ToTable("LOV_MASTER");
        builder.HasKey(e => e.LovId);
        builder.Property(e => e.LovType).HasColumnName("LOV_TYPE").HasColumnType("char(3)");
        builder.Property(e => e.LovId).HasColumnName("LOV_ID").ValueGeneratedNever();
        builder.Property(e => e.LovName).HasColumnName("LOV_NAME").HasMaxLength(200);
        builder.HasMany(e => e.UnitMappings).WithOne(e => e.LovMaster).HasForeignKey(e => e.LuLovId).HasPrincipalKey(e => e.LovId);
        builder.HasMany(e => e.PanelMappings).WithOne(e => e.LovMaster).HasForeignKey(e => e.LpLovId).HasPrincipalKey(e => e.LovId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class LovUnitMapConfiguration : IEntityTypeConfiguration<LovUnitMap>
{
    public void Configure(EntityTypeBuilder<LovUnitMap> builder)
    {
        builder.ToTable("LOV_UNITMAP");
        builder.HasKey(e => e.LuMapId);
        builder.Property(e => e.LuMapId).HasColumnName("LU_MAPID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.LuLovId).HasColumnName("LU_LOVID");
        builder.Property(e => e.LuUnitCode).HasColumnName("LU_UNITCODE").HasColumnType("char(3)");
        builder.Property(e => e.LuFlag).HasColumnName("LU_FLAG").HasColumnType("char(1)");
    }
}

public class LovPanelMapConfiguration : IEntityTypeConfiguration<LovPanelMap>
{
    public void Configure(EntityTypeBuilder<LovPanelMap> builder)
    {
        builder.ToTable("LOV_PANELMAP");
        builder.HasKey(e => new { e.LpLovId, e.LpPanelId });
        builder.Property(e => e.LpLovId).HasColumnName("LP_LOVID");
        builder.Property(e => e.LpPanelId).HasColumnName("LP_PANELID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.LpFlag).HasColumnName("LP_FLAG").HasColumnType("char(1)");
    }
}

public class PanelMasterConfiguration : IEntityTypeConfiguration<PanelMaster>
{
    public void Configure(EntityTypeBuilder<PanelMaster> builder)
    {
        builder.ToTable("PANEL_MAST");
        builder.HasKey(e => e.PanelId);
        builder.Property(e => e.PanelId).HasColumnName("PANEL_ID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.PanelName).HasColumnName("PANEL_NAME").HasMaxLength(65);
        builder.HasMany(e => e.PanelMappings).WithOne(e => e.PanelMaster).HasForeignKey(e => e.LpPanelId).HasPrincipalKey(e => e.PanelId);
    }
}

public class ProcessMasterConfiguration : IEntityTypeConfiguration<ProcessMaster>
{
    public void Configure(EntityTypeBuilder<ProcessMaster> builder)
    {
        builder.ToTable("PROCESS_MASTER");
        builder.HasKey(e => e.ProcessId);
        builder.Property(e => e.ProcessId).HasColumnName("PROCESS_ID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.ProcessName).HasColumnName("PROCESS_NAME").HasMaxLength(50);
        builder.Property(e => e.ProcessLivFlag).HasColumnName("PROCESS_LIVFLAG").HasColumnType("char(1)");
        builder.HasMany(e => e.UnitProcessMaps).WithOne(e => e.ProcessMaster).HasForeignKey(e => e.UpProcessId);
        builder.HasMany(e => e.AccessMasters).WithOne(e => e.ProcessMaster).HasForeignKey(e => e.UaProcessId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class UnitProcessMapConfiguration : IEntityTypeConfiguration<UnitProcessMap>
{
    public void Configure(EntityTypeBuilder<UnitProcessMap> builder)
    {
        builder.ToTable("UNIT_PROCESS_MAP");
        builder.HasKey(e => e.UpMapId);
        builder.Property(e => e.UpMapId).HasColumnName("UP_MAPID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.UpUnitCode).HasColumnName("UP_UNIT_CODE").HasColumnType("char(3)");
        builder.Property(e => e.UpProcessId).HasColumnName("UP_PROCESS_ID").HasColumnType("decimal(38,0)");
    }
}

public class UnitLovAccessMasterConfiguration : IEntityTypeConfiguration<UnitLovAccessMaster>
{
    public void Configure(EntityTypeBuilder<UnitLovAccessMaster> builder)
    {
        builder.ToTable("UNITLOV_ACCESSMAST");
        builder.HasKey(e => e.UaAccessMastId);
        builder.Property(e => e.UaAccessMastId).HasColumnName("UA_ACCESSMASTID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.UaUnitLovMapId).HasColumnName("UA_UNITLOVMAPID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UaDepartmentId).HasColumnName("UA_DEPARTMENTID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UaProcessId).HasColumnName("UA_PROCESSID").HasColumnType("decimal(38,0)");
        builder.HasMany(e => e.AccessDetails).WithOne(e => e.AccessMaster).HasForeignKey(e => e.UdAccessMastId);
        builder.HasOne(e => e.UnitLovMap).WithMany().HasForeignKey(e => e.UaUnitLovMapId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class UnitLovAccessDetailConfiguration : IEntityTypeConfiguration<UnitLovAccessDetail>
{
    public void Configure(EntityTypeBuilder<UnitLovAccessDetail> builder)
    {
        builder.ToTable("UNITLOV_ACCESSDET");
        builder.HasKey(e => e.UdAccessDetId);
        builder.Property(e => e.UdAccessDetId).HasColumnName("UD_ACCESSDETID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.UdAccessMastId).HasColumnName("UD_ACCESSMASTID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UdAccessType).HasColumnName("UD_ACCESSTYPE").HasColumnType("char(2)");
        builder.Property(e => e.UdEmpSysId).HasColumnName("UD_EMPSYSID").HasMaxLength(300);
        builder.Property(e => e.UdEscDays).HasColumnName("UD_ESCDAYS").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UdEffDat).HasColumnName("UD_EFF_DAT").HasMaxLength(255);
        builder.Property(e => e.UdClsDat).HasColumnName("UD_CLS_DAT").HasMaxLength(255);
        builder.Property(e => e.UdUpdatedBy).HasColumnName("UD_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UdUpdatedOn).HasColumnName("UD_UPDATEDON").HasColumnType("datetime2(3)");
    }
}
