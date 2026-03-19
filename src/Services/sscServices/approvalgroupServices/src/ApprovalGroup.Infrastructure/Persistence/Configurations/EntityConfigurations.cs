using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApprovalGroup.Domain.Entities;

namespace ApprovalGroup.Infrastructure.Persistence.Configurations;

public class ApprovalGroupMasterConfiguration : IEntityTypeConfiguration<ApprovalGroupMaster>
{
    public void Configure(EntityTypeBuilder<ApprovalGroupMaster> builder)
    {
        builder.ToTable("APGROUP_MAST");
        builder.HasKey(e => e.GroupId);
        builder.Property(e => e.GroupId).HasColumnName("GROUP_ID").ValueGeneratedNever();
        builder.Property(e => e.GroupName).HasColumnName("GROUP_NAME").HasMaxLength(50).IsRequired();
        builder.Property(e => e.GroupCreatedBy).HasColumnName("GROUP_CREATEDBY").IsRequired();
        builder.Property(e => e.GroupCreatedOn).HasColumnName("GROUP_CREATEDON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.GroupModifiedBy).HasColumnName("GROUP_MODIFIEDBY");
        builder.Property(e => e.GroupModifiedOn).HasColumnName("GROUP_MODIFIEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.GroupPriorityId).HasColumnName("GROUP_PRIORITYID");

        builder.HasMany(e => e.GroupMaps)
            .WithOne(m => m.ApprovalGroup)
            .HasForeignKey(m => m.MapGroupId);

        builder.HasMany(e => e.UserMaps)
            .WithOne(m => m.ApprovalGroup)
            .HasForeignKey(m => m.MapGroupId);
    }
}

public class ApprovalGroupMapConfiguration : IEntityTypeConfiguration<ApprovalGroupMap>
{
    public void Configure(EntityTypeBuilder<ApprovalGroupMap> builder)
    {
        builder.ToTable("APGROUP_MAP");
        builder.HasKey(e => e.MapId);
        builder.Property(e => e.MapId).HasColumnName("MAP_ID").ValueGeneratedNever();
        builder.Property(e => e.MapGroupId).HasColumnName("MAP_GROUPID").IsRequired();
        builder.Property(e => e.MapPayBySpecific).HasColumnName("MAP_PAYBYSPECIFIC").IsRequired();
        builder.Property(e => e.MapBuSpecific).HasColumnName("MAP_BUSPECIFIC").IsRequired();
        builder.Property(e => e.MapMainCat).HasColumnName("MAP_MAINCAT").IsRequired();
        builder.Property(e => e.MapSubCat).HasColumnName("MAP_SUBCAT").IsRequired();
        builder.Property(e => e.MapCurrency).HasColumnName("MAP_CURRENCY").HasColumnType("char(1)");
        builder.Property(e => e.MapCreatedBy).HasColumnName("MAP_CREATEDBY").IsRequired();
        builder.Property(e => e.MapCreatedOn).HasColumnName("MAP_CREATEDON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.MapModifiedBy).HasColumnName("MAP_MODIFIEDBY");
        builder.Property(e => e.MapModifiedOn).HasColumnName("MAP_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasMany(e => e.UnitMaps).WithOne(m => m.ApprovalGroupMap).HasForeignKey(m => m.MapGroupMapId);
        builder.HasMany(e => e.PayByMaps).WithOne(m => m.ApprovalGroupMap).HasForeignKey(m => m.MapGroupMapId);
        builder.HasMany(e => e.MainCatMaps).WithOne(m => m.ApprovalGroupMap).HasForeignKey(m => m.MapGroupMapId);
    }
}

public class ApprovalGroupUnitMapConfiguration : IEntityTypeConfiguration<ApprovalGroupUnitMap>
{
    public void Configure(EntityTypeBuilder<ApprovalGroupUnitMap> builder)
    {
        builder.ToTable("APGROUP_UNITMAP");
        builder.HasKey(e => e.MapId);
        builder.Property(e => e.MapId).HasColumnName("MAP_ID").ValueGeneratedNever();
        builder.Property(e => e.MapGroupMapId).HasColumnName("MAP_GROUMAPID").IsRequired();
        builder.Property(e => e.MapBuId).HasColumnName("MAP_BUID").HasMaxLength(25).IsRequired();
    }
}

public class ApprovalGroupPayByConfiguration : IEntityTypeConfiguration<ApprovalGroupPayBy>
{
    public void Configure(EntityTypeBuilder<ApprovalGroupPayBy> builder)
    {
        builder.ToTable("APGROUP_PAYBY");
        builder.HasKey(e => e.MapId);
        builder.Property(e => e.MapId).HasColumnName("MAP_ID").ValueGeneratedNever();
        builder.Property(e => e.MapGroupMapId).HasColumnName("MAP_GROUPMAPID").IsRequired();
        builder.Property(e => e.MapPayBy).HasColumnName("MAP_PAYBY").IsRequired();
    }
}

public class ApprovalGroupMainCatMapConfiguration : IEntityTypeConfiguration<ApprovalGroupMainCatMap>
{
    public void Configure(EntityTypeBuilder<ApprovalGroupMainCatMap> builder)
    {
        builder.ToTable("APGROUP_MAINCATMAP");
        builder.HasKey(e => e.MapId);
        builder.Property(e => e.MapId).HasColumnName("MAP_ID").ValueGeneratedNever();
        builder.Property(e => e.MapGroupMapId).HasColumnName("MAP_GROUPMAPID").IsRequired();
        builder.Property(e => e.MapMainCat).HasColumnName("MAP_MAINCAT").IsRequired();
    }
}

public class ApprovalGroupUserMapConfiguration : IEntityTypeConfiguration<ApprovalGroupUserMap>
{
    public void Configure(EntityTypeBuilder<ApprovalGroupUserMap> builder)
    {
        builder.ToTable("APGROUP_USERMAP");
        builder.HasKey(e => e.MapId);
        builder.Property(e => e.MapId).HasColumnName("MAP_ID").ValueGeneratedNever();
        builder.Property(e => e.MapGroupId).HasColumnName("MAP_GROUPID").IsRequired();
        builder.Property(e => e.MapUserId).HasColumnName("MAP_USERID").IsRequired();
        builder.Property(e => e.MapEffectiveDate).HasColumnName("MAP_EFFECTIVEDATE").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.MapClosureDate).HasColumnName("MAP_CLOSUREDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.MapCreatedBy).HasColumnName("MAP_CREATEDBY").IsRequired();
        builder.Property(e => e.MapCreatedOn).HasColumnName("MAP_CREATEDON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.MapModifiedBy).HasColumnName("MAP_MODIFIEDBY");
        builder.Property(e => e.MapModifiedOn).HasColumnName("MAP_MODIFIEDON").HasColumnType("datetime2(3)");
    }
}

public class PullMatrixDetailConfiguration : IEntityTypeConfiguration<PullMatrixDetail>
{
    public void Configure(EntityTypeBuilder<PullMatrixDetail> builder)
    {
        builder.ToTable("PULLMATRIX_DET");
        builder.HasKey(e => e.MatId);
        builder.Property(e => e.MatId).HasColumnName("MAT_ID").ValueGeneratedNever();
        builder.Property(e => e.MatUnitId).HasColumnName("MAT_UNITID").IsRequired();
        builder.Property(e => e.MatPayBy).HasColumnName("MAT_PAYBY").HasColumnType("char(2)").IsRequired();
        builder.Property(e => e.MatFlag).HasColumnName("MAT_FLAG").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.MatMainCat).HasColumnName("MAT_MAINCAT").IsRequired();
        builder.Property(e => e.MatEmpSysId).HasColumnName("MAT_EMPSYSID").IsRequired();
        builder.Property(e => e.MatMaxNos).HasColumnName("MAT_MAXNOS").IsRequired();
        builder.Property(e => e.MatCreatedBy).HasColumnName("MAT_CREATEDBY").IsRequired();
        builder.Property(e => e.MatCreatedOn).HasColumnName("MAT_CREATEDON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.MatModifiedBy).HasColumnName("MAT_MODIFIEDBY").IsRequired();
        builder.Property(e => e.MatModifiedOn).HasColumnName("MAT_MODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
    }
}
