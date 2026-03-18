using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CompetencyService.Domain.Entities;

namespace CompetencyService.Infrastructure.Persistence.Configurations;

public class CompetencyMasterConfiguration : IEntityTypeConfiguration<CompetencyMaster>
{
    public void Configure(EntityTypeBuilder<CompetencyMaster> builder)
    {
        builder.ToTable("DD_COMPENDMAST");
        builder.HasNoKey(); // Uses decimal PK mapping
        builder.Property(e => e.Id).HasColumnName("CM_CPD_NUM").HasColumnType("decimal(38,0)");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasColumnName("CM_CPD_NAM").HasColumnType("varchar(4000)").IsRequired();
        builder.Property(e => e.EffectiveDate).HasColumnName("CM_EFF_DAT").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.ClosureDate).HasColumnName("CM_CLS_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.Remarks).HasColumnName("CM_CPD_REM").HasColumnType("varchar(4000)");
        builder.Property(e => e.JobCode).HasColumnName("CM_JOB_COD").HasColumnType("decimal(38,0)");
        builder.Property(e => e.PositiveIndicator).HasColumnName("CM_POS_IND").HasColumnType("varchar(4000)");
        builder.Property(e => e.NegativeIndicator).HasColumnName("CM_NEG_IND").HasColumnType("varchar(4000)");
        builder.Property(e => e.SelfDescription).HasColumnName("CM_CPD_SLF").HasColumnType("varchar(4000)");
        builder.Property(e => e.CompetencyType).HasColumnName("CM_CPD_TYPE").HasColumnType("varchar(10)");
        builder.Property(e => e.ParentId).HasColumnName("CM_PARENTID").HasColumnType("decimal(38,0)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class CompetencyIndicatorConfiguration : IEntityTypeConfiguration<CompetencyIndicator>
{
    public void Configure(EntityTypeBuilder<CompetencyIndicator> builder)
    {
        builder.ToTable("DD_COMPETENCY_IND");
        builder.HasNoKey();
        builder.Property(e => e.SerialNo).HasColumnName("SRL_NO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Band).HasColumnName("BAND").HasColumnType("varchar(50)");
        builder.Property(e => e.CompetencyNo).HasColumnName("COMP_NUM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.IndicatorFlag).HasColumnName("IND_FLAG").HasColumnType("char(1)");
        builder.Property(e => e.IndicatorDefinition).HasColumnName("IND_DEFN").HasColumnType("varchar(4000)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class CompetencyRatingScaleConfiguration : IEntityTypeConfiguration<CompetencyRatingScale>
{
    public void Configure(EntityTypeBuilder<CompetencyRatingScale> builder)
    {
        builder.ToTable("COMPETENCY_RATING_SCALE");
        builder.HasKey(e => e.CompetencyId);
        builder.Property(e => e.CompetencyId).HasColumnName("COMPETENCY_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.R1Desc).HasColumnName("R1_DESC").HasColumnType("varchar(250)").IsRequired();
        builder.Property(e => e.R2Desc).HasColumnName("R2_DESC").HasColumnType("varchar(500)");
        builder.Property(e => e.R3Desc).HasColumnName("R3_DESC").HasColumnType("varchar(500)").IsRequired();
        builder.Property(e => e.R4Desc).HasColumnName("R4_DESC").HasColumnType("varchar(500)");
        builder.Property(e => e.R5Desc).HasColumnName("R5_DESC").HasColumnType("varchar(500)").IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class BandCoreCompetencyConfiguration : IEntityTypeConfiguration<BandCoreCompetency>
{
    public void Configure(EntityTypeBuilder<BandCoreCompetency> builder)
    {
        builder.ToTable("BAND_CORECOMPETENCY");
        builder.HasKey(e => new { e.BandId, e.CompetencyId });
        builder.Property(e => e.BandId).HasColumnName("BAND_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CompetencyId).HasColumnName("COMPETENCY_ID").HasColumnType("decimal(38,0)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class EmpSpecificCompetencyConfiguration : IEntityTypeConfiguration<EmpSpecificCompetency>
{
    public void Configure(EntityTypeBuilder<EmpSpecificCompetency> builder)
    {
        builder.ToTable("EMP_SPECIFIC_COMPETENCY");
        builder.HasKey(e => new { e.EmpSysId, e.CompetencyId, e.CompetencyType, e.YearId });
        builder.Property(e => e.EmpSysId).HasColumnName("EMP_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CompetencyId).HasColumnName("COMPETENCY_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CompetencyType).HasColumnName("COMPETENCY_TYPE").HasColumnType("char(1)");
        builder.Property(e => e.YearId).HasColumnName("DD_YEARID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class RoleSpecificConfiguration : IEntityTypeConfiguration<RoleSpecific>
{
    public void Configure(EntityTypeBuilder<RoleSpecific> builder)
    {
        builder.ToTable("ROLE_SPECIFIC");
        builder.HasKey(e => new { e.EmpSysId, e.CompetencyId });
        builder.Property(e => e.EmpSysId).HasColumnName("EMP_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CompetencyId).HasColumnName("COMPETENCY_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.EffFrom).HasColumnName("EFF_FROM").HasColumnType("datetime2(3)");
        builder.Property(e => e.EffTo).HasColumnName("EFF_TO").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class VtcCompetencyConfiguration : IEntityTypeConfiguration<VtcCompetency>
{
    public void Configure(EntityTypeBuilder<VtcCompetency> builder)
    {
        builder.ToTable("DD_VTCCOMPETENCY");
        builder.HasNoKey();
        builder.Property(e => e.SerialNo).HasColumnName("SRL_NO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Band).HasColumnName("BAND").HasColumnType("varchar(50)");
        builder.Property(e => e.CompetencyNo).HasColumnName("COMP_NUM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CompetencyName).HasColumnName("COMP_NAM").HasColumnType("varchar(50)");
        builder.Ignore(e => e.DomainEvents);
    }
}
