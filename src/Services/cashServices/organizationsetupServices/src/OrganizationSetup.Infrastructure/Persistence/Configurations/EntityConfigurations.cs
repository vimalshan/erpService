using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationSetup.Domain.Entities;
using OrganizationSetup.Domain.ValueObjects;

namespace OrganizationSetup.Infrastructure.Persistence.Configurations;

public class DealRoleConfiguration : IEntityTypeConfiguration<DealRole>
{
    public void Configure(EntityTypeBuilder<DealRole> builder)
    {
        builder.HasKey(x => x.RoleId);
        builder.Property(x => x.RoleId).HasColumnName("ROLE_ID");
        builder.Property(x => x.RoleName).HasColumnName("ROLE_NAME").HasMaxLength(50)
            .HasConversion(rm => rm.Value, s => RoleName.Create(s));
        builder.Property(x => x.RoleLevel).HasColumnName("ROLE_LEVEL");
        builder.Property(x => x.RoleModifiedBy).HasColumnName("ROLE_MODIFIEDBY");
        builder.Property(x => x.RoleModifiedOn).HasColumnName("ROLE_MODIFIEDON");

        builder.HasIndex(x => x.RoleName).HasDatabaseName("IX_DEAL_ROLE_NAME");
        builder.ToTable("DEAL_ROLE");
    }
}

public class DealUserMapConfiguration : IEntityTypeConfiguration<DealUserMap>
{
    public void Configure(EntityTypeBuilder<DealUserMap> builder)
    {
        builder.HasKey(x => x.RoleMapId);
        builder.Property(x => x.RoleMapId).HasColumnName("ROLE_MAPID");
        builder.Property(x => x.RoleId).HasColumnName("ROLE_ID");
        builder.Property(x => x.RoleEmpSysId).HasColumnName("ROLE_EMPSYSID");
        builder.Property(x => x.RoleOrgId).HasColumnName("ROLE_ORGID");
        builder.Property(x => x.RoleBusiness).HasColumnName("ROLE_BUSINESS");

        builder.HasOne(x => x.Role).WithMany(r => r.UserMaps).HasForeignKey(x => x.RoleId)
            .HasConstraintName("FK_DEAL_USERMAP_ROLE");

        builder.HasIndex(x => x.RoleEmpSysId).HasDatabaseName("IX_DEAL_USERMAP_EMPID");
        builder.HasIndex(x => x.RoleOrgId).HasDatabaseName("IX_DEAL_USERMAP_ORGID");
        builder.ToTable("DEAL_USERMAP");
    }
}

public class DealOrgParamsConfiguration : IEntityTypeConfiguration<DealOrgParams>
{
    public void Configure(EntityTypeBuilder<DealOrgParams> builder)
    {
        builder.HasKey(x => x.OrgParamId);
        builder.Property(x => x.OrgParamId).HasColumnName("ORG_PARAMID");
        builder.Property(x => x.OrgParamType).HasColumnName("ORG_PARAMTYPE").HasMaxLength(6)
            .HasConversion(pt => pt.Value, s => ParameterType.Create(s));
        builder.Property(x => x.OrgParamValue).HasColumnName("ORG_PARAMVALUE");
        builder.Property(x => x.OrgId).HasColumnName("ORG_ID");
        builder.Property(x => x.OrgModifiedBy).HasColumnName("ORG_MODIFIEDBY");
        builder.Property(x => x.OrgModifiedOn).HasColumnName("ORG_MODIFIEDON");

        builder.HasIndex(x => x.OrgId).HasDatabaseName("IX_DEAL_ORGPARAMS_ORGID");
        builder.HasIndex(x => x.OrgParamType).HasDatabaseName("IX_DEAL_ORGPARAMS_PARAMTYPE");
        builder.ToTable("DEAL_ORGPARAMS");
    }
}

public class DealPpLimitConfiguration : IEntityTypeConfiguration<DealPpLimit>
{
    public void Configure(EntityTypeBuilder<DealPpLimit> builder)
    {
        builder.HasKey(x => x.PpLimitId);
        builder.Property(x => x.PpLimitId).HasColumnName("PP_LIMITID");
        builder.Property(x => x.PpOrgId).HasColumnName("PP_ORGID");
        builder.Property(x => x.PpTranType).HasColumnName("PP_TRANTYPE").HasMaxLength(1)
            .HasConversion(tt => tt.Value, s => TransactionType.Create(s));
        builder.Property(x => x.PpBasCurr).HasColumnName("PP_BASCURR");
        builder.Property(x => x.PpLimitAmt).HasColumnName("PP_LIMITAMT").HasPrecision(19, 0);
        builder.Property(x => x.PpFinYear).HasColumnName("PP_FINYEAR");
        builder.Property(x => x.PpLimitAct).HasColumnName("PP_LIMITACT").HasPrecision(19, 0);
        builder.Property(x => x.PpCertificateUpload).HasColumnName("PP_CERTIFICATEUPLOAD").HasMaxLength(500);
        builder.Property(x => x.PpModifiedBy).HasColumnName("PP_MODIFIEDBY");
        builder.Property(x => x.PpModifiedOn).HasColumnName("PP_MODIFIEDON");

        builder.HasIndex(x => x.PpOrgId).HasDatabaseName("IX_DEAL_PPLIMIT_ORGID");
        builder.HasIndex(x => x.PpFinYear).HasDatabaseName("IX_DEAL_PPLIMIT_FINYEAR");
        builder.ToTable("DEAL_PPLIMIT");
    }
}
