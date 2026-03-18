using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReferenceService.Domain;
using ReferenceService.Domain.Entities;

namespace ReferenceService.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for LovType entity.
/// </summary>
public class LovTypeConfiguration : IEntityTypeConfiguration<LovType>
{
    public void Configure(EntityTypeBuilder<LovType> builder)
    {
        builder.ToTable("LOV_TYPEMAST");
        
        builder.HasKey(x => x.Id)
            .HasName("PK_LOV_TYPEMAST");
        
        builder.Property(x => x.Id)
            .HasColumnName("LOV_TYPEID")
            .ValueGeneratedNever();
        
        builder.Property(x => x.TypeName)
            .HasColumnName("LOV_TYPENAME")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasColumnName("LOV_DESCRIPTION")
            .HasMaxLength(500);
        
        builder.Property(x => x.Sequence)
            .HasColumnName("LOV_TYPESEQ")
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasColumnName("LOV_STATUS")
            .HasConversion(x => x == EntityStatus.Active ? 'Y' : 'N',
                x => x == 'Y' ? EntityStatus.Active : EntityStatus.Inactive);
        
        builder.Property(x => x.LastModifiedBy)
            .HasColumnName("LOV_LASTMODIFIEDBY")
            .IsRequired();
        
        builder.Property(x => x.LastModifiedOn)
            .HasColumnName("LOV_LASTMODIFIEDON")
            .HasColumnType("DATETIME2(3)")
            .IsRequired();
        
        builder.HasIndex(x => x.TypeName)
            .HasDatabaseName("UQ_LOV_TYPENAME")
            .IsUnique();
        
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_LOV_TYPEMAST_STATUS");
        
        // Navigation
        builder.HasMany<LovValue>()
            .WithOne()
            .HasForeignKey(x => x.TypeId)
            .HasConstraintName("FK_LOV_TYPEID");
    }
}

/// <summary>
/// EF Core configuration for LovValue entity.
/// </summary>
public class LovValueConfiguration : IEntityTypeConfiguration<LovValue>
{
    public void Configure(EntityTypeBuilder<LovValue> builder)
    {
        builder.ToTable("LOV_MAST");
        
        builder.HasKey(x => x.Id)
            .HasName("PK_LOV_MAST");
        
        builder.Property(x => x.Id)
            .HasColumnName("LOV_ID")
            .ValueGeneratedNever();
        
        builder.Property(x => x.TypeId)
            .HasColumnName("LOV_TYPEID")
            .IsRequired();
        
        builder.Property(x => x.Code)
            .HasColumnName("LOV_CODE")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasColumnName("LOV_DESCRIPTION")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.LongDescription)
            .HasColumnName("LOV_LONGDESCRIPTION")
            .HasMaxLength(500);
        
        builder.Property(x => x.Sequence)
            .HasColumnName("LOV_SEQUENCE")
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasColumnName("LOV_STATUS")
            .HasConversion(x => x == EntityStatus.Active ? 'Y' : 'N',
                x => x == 'Y' ? EntityStatus.Active : EntityStatus.Inactive);
        
        builder.Property(x => x.LastModifiedBy)
            .HasColumnName("LOV_LASTMODIFIEDBY")
            .IsRequired();
        
        builder.Property(x => x.LastModifiedOn)
            .HasColumnName("LOV_LASTMODIFIEDON")
            .HasColumnType("DATETIME2(3)")
            .IsRequired();
        
        builder.HasIndex(x => x.TypeId)
            .HasDatabaseName("IX_LOV_MAST_TYPEID");
        
        builder.HasIndex(x => x.Code)
            .HasDatabaseName("IX_LOV_MAST_CODE");
        
        builder.HasIndex(x => new { x.TypeId, x.Code })
            .HasDatabaseName("UQ_LOV_CODE")
            .IsUnique();
    }
}

/// <summary>
/// EF Core configuration for PermissionRule entity.
/// </summary>
public class PermissionRuleConfiguration : IEntityTypeConfiguration<PermissionRule>
{
    public void Configure(EntityTypeBuilder<PermissionRule> builder)
    {
        builder.ToTable("PERMISSION_RULES");
        
        builder.HasKey(x => x.Id)
            .HasName("PK_PERMISSION_RULES");
        
        builder.Property(x => x.Id)
            .HasColumnName("PERM_ID")
            .ValueGeneratedNever();
        
        builder.Property(x => x.ResourceId)
            .HasColumnName("PERM_RESOURCEID")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.Action)
            .HasColumnName("PERM_ACTION")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasColumnName("PERM_DESCRIPTION")
            .HasMaxLength(255);
        
        builder.Property(x => x.AppCode)
            .HasColumnName("PERM_APPCODE")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasColumnName("PERM_STATUS")
            .HasConversion(x => x == EntityStatus.Active ? 'Y' : 'N',
                x => x == 'Y' ? EntityStatus.Active : EntityStatus.Inactive);
        
        builder.Property(x => x.LastModifiedBy)
            .HasColumnName("PERM_LASTMODIFIEDBY")
            .IsRequired();
        
        builder.Property(x => x.LastModifiedOn)
            .HasColumnName("PERM_LASTMODIFIEDON")
            .HasColumnType("DATETIME2(3)")
            .IsRequired();
        
        builder.HasIndex(x => new { x.ResourceId, x.Action, x.AppCode })
            .HasDatabaseName("UQ_PERMISSION")
            .IsUnique();
    }
}

/// <summary>
/// EF Core configuration for LeaveFlag entity.
/// </summary>
public class LeaveFlagConfiguration : IEntityTypeConfiguration<LeaveFlag>
{
    public void Configure(EntityTypeBuilder<LeaveFlag> builder)
    {
        builder.ToTable("LEAVEFLAG");
        
        builder.HasKey(x => x.Id)
            .HasName("PK_LEAVEFLAG");
        
        builder.Property(x => x.Id)
            .HasColumnName("LEAVEFLAG_ID")
            .ValueGeneratedNever();
        
        builder.Property(x => x.Code)
            .HasColumnName("LEAVEFLAG_CODE")
            .HasMaxLength(10)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasColumnName("LEAVEFLAG_DESCRIPTION")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Type)
            .HasColumnName("LEAVEFLAG_TYPE")
            .HasMaxLength(50);
        
        builder.Property(x => x.Status)
            .HasColumnName("LEAVEFLAG_STATUS")
            .HasConversion(x => x == EntityStatus.Active ? 'Y' : 'N',
                x => x == 'Y' ? EntityStatus.Active : EntityStatus.Inactive);
        
        builder.Property(x => x.LastModifiedBy)
            .HasColumnName("LEAVEFLAG_LASTMODIFIEDBY")
            .IsRequired();
        
        builder.Property(x => x.LastModifiedOn)
            .HasColumnName("LEAVEFLAG_LASTMODIFIEDON")
            .HasColumnType("DATETIME2(3)")
            .IsRequired();
        
        builder.HasIndex(x => x.Code)
            .HasDatabaseName("IX_LEAVEFLAG_CODE")
            .IsUnique();
    }
}
