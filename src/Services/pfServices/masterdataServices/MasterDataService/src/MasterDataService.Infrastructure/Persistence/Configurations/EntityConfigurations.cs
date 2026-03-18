using MasterDataService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterDataService.Infrastructure.Persistence.Configurations;

public class LovMasterConfiguration : IEntityTypeConfiguration<LovMaster>
{
    public void Configure(EntityTypeBuilder<LovMaster> builder)
    {
        builder.ToTable("LOV_MASTER");
        builder.HasKey(x => x.LovId);
        builder.Property(x => x.LovId).HasColumnName("LOV_ID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.LovCode).HasColumnName("LOV_CODE").HasMaxLength(10).IsRequired();
        builder.Property(x => x.LovDescription).HasColumnName("LOV_DESC").HasMaxLength(100).IsRequired();
        builder.Property(x => x.LovValue).HasColumnName("LOV_VALUE").HasMaxLength(20).IsRequired();
        builder.Property(x => x.LovCategory).HasColumnName("LOV_CATEGORY").HasMaxLength(50).IsRequired();
        builder.Property(x => x.LovStatus).HasColumnName("LOV_STATUS").HasMaxLength(1).HasDefaultValue("A");
        builder.HasIndex(x => new { x.LovCategory, x.LovCode }).HasDatabaseName("IDX_LOV_MASTER_CODE");
    }
}

public class ConfigurationConfiguration : IEntityTypeConfiguration<Configuration>
{
    public void Configure(EntityTypeBuilder<Configuration> builder)
    {
        builder.ToTable("CONFIGURATION");
        builder.HasKey(x => x.ConfigId);
        builder.Property(x => x.ConfigId).HasColumnName("CONFIG_ID").UseIdentityColumn();
        builder.Property(x => x.ConfigKey).HasColumnName("CONFIG_KEY").HasMaxLength(100).IsRequired();
        builder.Property(x => x.ConfigValue).HasColumnName("CONFIG_VALUE").HasMaxLength(500).IsRequired();
        builder.Property(x => x.ConfigType).HasColumnName("CONFIG_TYPE").HasMaxLength(50).IsRequired();
        builder.Property(x => x.ConfigDescription).HasColumnName("CONFIG_DESCRIPTION").HasMaxLength(200);
        builder.Property(x => x.CreatedDate).HasColumnName("CREATED_DATE");
        builder.Property(x => x.UpdatedDate).HasColumnName("UPDATED_DATE");
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY");
        builder.HasIndex(x => x.ConfigKey).IsUnique().HasDatabaseName("IDX_CONFIG_KEY");
    }
}

public class RateMasterConfiguration : IEntityTypeConfiguration<RateMaster>
{
    public void Configure(EntityTypeBuilder<RateMaster> builder)
    {
        builder.ToTable("RATE_MASTER");
        builder.HasKey(x => new { x.TrustCode, x.RateId });
        builder.Property(x => x.TrustCode).HasColumnName("RT_TRUST_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(x => x.RateId).HasColumnName("RATE_ID");
        builder.Property(x => x.RateTypeCode).HasColumnName("RT_RATE_TYPE_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(x => x.RateEffectiveDate).HasColumnName("RATE_EFF_DATE").HasMaxLength(255);
        builder.Property(x => x.RateClosingDate).HasColumnName("RATE_CLS_DATE").HasMaxLength(255);
        builder.Property(x => x.RateValue).HasColumnName("RATE_VALUE").HasColumnType("decimal(19,0)");
        builder.Property(x => x.RateDeleteFlag).HasColumnName("RATE_DEL_FLAG").HasMaxLength(1).IsFixedLength();
        builder.Property(x => x.ReworkStatus).HasColumnName("RT_REWRK_STS").HasMaxLength(1).IsFixedLength();
        builder.HasIndex(x => new { x.RateTypeCode, x.RateEffectiveDate }).HasDatabaseName("IDX_RATE_MASTER_TYPE");
        builder.HasOne(x => x.RateType).WithMany().HasForeignKey(x => x.RateTypeCode).HasPrincipalKey(x => x.RateTypeCode).IsRequired(false);
    }
}

public class FinancialYearRuleConfiguration : IEntityTypeConfiguration<FinancialYearRule>
{
    public void Configure(EntityTypeBuilder<FinancialYearRule> builder)
    {
        builder.ToTable("PF_FINYEARRULES");
        builder.HasKey(x => x.FinYearCode);
        builder.Property(x => x.FinYearCode).HasColumnName("PF_FINYEAR_CODE").ValueGeneratedNever();
        builder.Property(x => x.FinYearRules).HasColumnName("PF_FINYEAR_RULES").HasMaxLength(4000);
    }
}

public class FundTypeConfiguration : IEntityTypeConfiguration<FundType>
{
    public void Configure(EntityTypeBuilder<FundType> builder)
    {
        builder.ToTable("FUND_TYPE_MASTER");
        builder.HasKey(x => x.FundTypeCode);
        builder.Property(x => x.FundTypeCode).HasColumnName("FUND_TYPECODE").HasMaxLength(3).IsFixedLength();
        builder.Property(x => x.FundTypeName).HasColumnName("FUND_TYPENAME").HasMaxLength(25).IsRequired();
    }
}

public class StatusMasterConfiguration : IEntityTypeConfiguration<StatusMaster>
{
    public void Configure(EntityTypeBuilder<StatusMaster> builder)
    {
        builder.ToTable("STATUS_MASTER");
        builder.HasKey(x => new { x.StatusType, x.StatusCodeValue });
        builder.Property(x => x.StatusType).HasColumnName("STATUS_TYPE").HasMaxLength(2).IsFixedLength();
        builder.Property(x => x.StatusCodeValue).HasColumnName("STATUS_CODE").HasMaxLength(2).IsFixedLength();
        builder.Property(x => x.StatusName).HasColumnName("STATUS_NAME").HasMaxLength(65);
    }
}

public class RoleMasterConfiguration : IEntityTypeConfiguration<RoleMaster>
{
    public void Configure(EntityTypeBuilder<RoleMaster> builder)
    {
        builder.ToTable("ROLE_MASTER");
        builder.HasKey(x => x.RoleCode);
        builder.Property(x => x.RoleCode).HasColumnName("ROLE_CODE").ValueGeneratedNever();
        builder.Property(x => x.RoleName).HasColumnName("ROLE_NAME").HasMaxLength(65).IsRequired();
        builder.Property(x => x.RoleDescription).HasColumnName("ROLE_DESCRIPTION").HasMaxLength(200);
        builder.Property(x => x.RoleStatus).HasColumnName("ROLE_STATUS").HasMaxLength(1).HasDefaultValue("A");
    }
}

public class RateTypeConfiguration : IEntityTypeConfiguration<RateType>
{
    public void Configure(EntityTypeBuilder<RateType> builder)
    {
        builder.ToTable("RATE_TYPE_MASTER");
        builder.HasKey(x => x.RateTypeCode);
        builder.Property(x => x.RateTypeCode).HasColumnName("RATE_TYPE_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(x => x.RateTypeName).HasColumnName("RATE_TYPE_NAME").HasMaxLength(25);
    }
}

public class ComputationMonthConfiguration : IEntityTypeConfiguration<ComputationMonth>
{
    public void Configure(EntityTypeBuilder<ComputationMonth> builder)
    {
        builder.ToTable("COMP_MONTH");
        builder.HasKey(x => x.SerialNumber);
        builder.Property(x => x.SerialNumber).HasColumnName("AC_SRL_NUM").ValueGeneratedNever();
        builder.Property(x => x.MonthName).HasColumnName("AC_MNT_NAM").HasMaxLength(15);
    }
}

public class ComputationFinancialYearConfiguration : IEntityTypeConfiguration<ComputationFinancialYear>
{
    public void Configure(EntityTypeBuilder<ComputationFinancialYear> builder)
    {
        builder.ToTable("COMP_FINYEAR");
        builder.HasKey(x => x.SerialNumber);
        builder.Property(x => x.SerialNumber).HasColumnName("AC_SRL_NUM").ValueGeneratedNever();
        builder.Property(x => x.StartDate).HasColumnName("AC_STR_DAT");
        builder.Property(x => x.EndDate).HasColumnName("AC_END_DAT");
        builder.Property(x => x.CloseFlag).HasColumnName("AC_CLS_FLG").HasMaxLength(1).IsFixedLength().IsRequired();
        builder.Property(x => x.Remarks).HasColumnName("AC_REMARKS").HasMaxLength(4000);
        builder.Property(x => x.InterestFlag).HasColumnName("AC_INT_FLG").HasMaxLength(1).IsFixedLength();
        builder.Property(x => x.EmployeeName).HasColumnName("AC_EMP_NAME").HasMaxLength(65);
        builder.Property(x => x.EmployeeDesignation).HasColumnName("AC_EMP_DESG").HasMaxLength(65);
        builder.Property(x => x.BatchNumber).HasColumnName("AC_BAT_NUM");
    }
}

public class InvestmentCategoryGroupConfiguration : IEntityTypeConfiguration<InvestmentCategoryGroup>
{
    public void Configure(EntityTypeBuilder<InvestmentCategoryGroup> builder)
    {
        builder.ToTable("INVCATGRP_MAST");
        builder.HasKey(x => x.GroupId);
        builder.Property(x => x.GroupId).HasColumnName("INVGRP_ID").ValueGeneratedNever();
        builder.Property(x => x.ShortName).HasColumnName("INVGRP_SHTNAME").HasMaxLength(20);
        builder.Property(x => x.GroupName).HasColumnName("INVGRP_NAME").HasMaxLength(50);
        builder.HasMany(x => x.GroupLimits).WithOne(x => x.Group).HasForeignKey(x => x.GroupId);
    }
}

public class InvestmentCategoryLimitConfiguration : IEntityTypeConfiguration<InvestmentCategoryLimit>
{
    public void Configure(EntityTypeBuilder<InvestmentCategoryLimit> builder)
    {
        builder.ToTable("INVCAT_LIMIT");
        builder.HasKey(x => x.LimitId);
        builder.Property(x => x.LimitId).HasColumnName("INVCAT_LIMITID").ValueGeneratedNever();
        builder.Property(x => x.CategoryId).HasColumnName("INVCAT_ID");
        builder.Property(x => x.MaxPercentage).HasColumnName("INVCAT_MAXPER");
        builder.Property(x => x.EffectiveDate).HasColumnName("INVCAT_EFFDATE");
        builder.Property(x => x.ClosingDate).HasColumnName("INVCAT_CLSDATE");
    }
}

public class InvestmentGroupLimitConfiguration : IEntityTypeConfiguration<InvestmentGroupLimit>
{
    public void Configure(EntityTypeBuilder<InvestmentGroupLimit> builder)
    {
        builder.ToTable("INVGRP_LIMIT");
        builder.HasKey(x => x.LimitId);
        builder.Property(x => x.LimitId).HasColumnName("INVGRP_LIMITID").ValueGeneratedNever();
        builder.Property(x => x.GroupId).HasColumnName("INVGRP_ID");
        builder.Property(x => x.MaxPercentage).HasColumnName("INVGRP_MAXPER");
        builder.Property(x => x.EffectiveDate).HasColumnName("INVGRP_EFFDATE");
        builder.Property(x => x.ClosingDate).HasColumnName("INVGRP_CLSDATE");
        builder.Property(x => x.Range).HasColumnName("INVGRP_RANGE").HasMaxLength(20);
    }
}

public class PfHrisConfiguration : IEntityTypeConfiguration<PfHris>
{
    public void Configure(EntityTypeBuilder<PfHris> builder)
    {
        builder.ToTable("PF_HRIS");
        builder.HasKey(x => new { x.CompanyCode, x.EmployeeNumber });
        builder.Property(x => x.CompanyCode).HasColumnName("COM_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(x => x.EmployeeNumber).HasColumnName("EMP_NUM").HasColumnType("decimal(38,0)");
        builder.Property(x => x.PinNumber).HasColumnName("PIN_NUM").HasColumnType("decimal(38,0)");
    }
}

public class PfMainAccountConfiguration : IEntityTypeConfiguration<PfMainAccount>
{
    public void Configure(EntityTypeBuilder<PfMainAccount> builder)
    {
        builder.ToTable("PF_MAIN_ACCOUNT");
        builder.HasKey(x => x.MainAccountCode);
        builder.Property(x => x.MainAccountCode).HasColumnName("MAIN_ACC_COD").HasColumnType("decimal(38,0)");
        builder.Property(x => x.MainAccountName).HasColumnName("MAIN_ACC_NAM").HasMaxLength(60).IsRequired();
        builder.HasMany(x => x.SubMappings).WithOne(x => x.MainAccount).HasForeignKey(x => x.MainAccountCode).HasPrincipalKey(x => x.MainAccountCode);
    }
}

public class PfMainSubMappingConfiguration : IEntityTypeConfiguration<PfMainSubMapping>
{
    public void Configure(EntityTypeBuilder<PfMainSubMapping> builder)
    {
        builder.ToTable("PF_MAIN_SUB");
        builder.HasKey(x => new { x.MainAccountCode, x.SubAccountCode });
        builder.Property(x => x.MainAccountCode).HasColumnName("MAIN_ACC_COD").HasColumnType("decimal(38,0)");
        builder.Property(x => x.SubAccountCode).HasColumnName("SUB_ACC_COD").HasColumnType("decimal(38,0)");
    }
}
