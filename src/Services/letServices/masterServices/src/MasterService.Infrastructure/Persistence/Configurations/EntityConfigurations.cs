using MasterService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterService.Infrastructure.Persistence.Configurations;

public class JobMasterConfiguration : IEntityTypeConfiguration<JobMaster>
{
    public void Configure(EntityTypeBuilder<JobMaster> builder)
    {
        builder.ToTable("JOB_MAST");
        builder.HasKey(j => j.JobCode);
        builder.Property(j => j.JobCode).HasColumnName("JB_JOB_COD").ValueGeneratedNever();
        builder.Property(j => j.JobName).HasColumnName("JB_JOB_NAM").HasMaxLength(65).IsRequired();
        builder.Property(j => j.CategoryCode).HasColumnName("JB_CAT_COD").HasMaxLength(3).IsRequired().IsFixedLength();
        builder.Property(j => j.SerialNumber).HasColumnName("JB_SRL_NUM");
        builder.HasIndex(j => j.CategoryCode).HasDatabaseName("IDX_JOB_MAST_CAT_COD");
        builder.Ignore(j => j.DomainEvents);
    }
}

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("CAT_MAST");
        builder.HasKey(c => c.CategoryCode);
        builder.Property(c => c.CategoryCode).HasColumnName("CT_CAT_COD").HasMaxLength(3).IsRequired().IsFixedLength();
        builder.Property(c => c.CategoryName).HasColumnName("CT_CAT_NAM").HasMaxLength(65).IsRequired();
        builder.Property(c => c.SerialNumber).HasColumnName("CT_SRL_NUM");
        builder.Ignore(c => c.DomainEvents);
    }
}

public class CompanyFinancialYearConfiguration : IEntityTypeConfiguration<CompanyFinancialYear>
{
    public void Configure(EntityTypeBuilder<CompanyFinancialYear> builder)
    {
        builder.ToTable("COMP_FINYEAR");
        builder.HasKey(f => f.SerialNumber);
        builder.Property(f => f.SerialNumber).HasColumnName("AC_SRL_NUM").ValueGeneratedNever();
        builder.Property(f => f.StartDate).HasColumnName("AC_STR_DAT").IsRequired();
        builder.Property(f => f.EndDate).HasColumnName("AC_END_DAT").IsRequired();
        builder.Property(f => f.CloseFlag).HasColumnName("AC_CLS_FLG").HasMaxLength(1).IsRequired();
        builder.HasIndex(f => f.StartDate).HasDatabaseName("IDX_COMP_FINYEAR_STR_DAT");
        builder.Ignore(f => f.DomainEvents);
        builder.Ignore(f => f.IsOpen);
    }
}

public class BenefitConfiguration : IEntityTypeConfiguration<Benefit>
{
    public void Configure(EntityTypeBuilder<Benefit> builder)
    {
        builder.ToTable("BENEFIT_MAST");
        builder.HasKey(b => b.BenefitCode);
        builder.Property(b => b.BenefitCode).HasColumnName("BE_BEN_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(b => b.BenefitDescription).HasColumnName("BE_BEN_DES").HasMaxLength(200).IsRequired();
        builder.Ignore(b => b.DomainEvents);
    }
}

public class CostMasterConfiguration : IEntityTypeConfiguration<CostMaster>
{
    public void Configure(EntityTypeBuilder<CostMaster> builder)
    {
        builder.ToTable("COST_MAST");
        builder.HasKey(c => c.CostCode);
        builder.Property(c => c.CostCode).HasColumnName("CS_CST_COD").ValueGeneratedNever();
        builder.Property(c => c.CostName).HasColumnName("CS_CST_NAM").HasMaxLength(65).IsRequired();
        builder.Ignore(c => c.DomainEvents);
    }
}

public class FunctionMasterConfiguration : IEntityTypeConfiguration<FunctionMaster>
{
    public void Configure(EntityTypeBuilder<FunctionMaster> builder)
    {
        builder.ToTable("FUNCTION_MAST");
        builder.HasKey(f => f.FunctionCode);
        builder.Property(f => f.FunctionCode).HasColumnName("FN_FNC_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(f => f.FunctionName).HasColumnName("FN_FNC_NAM").HasMaxLength(65).IsRequired();
        builder.Property(f => f.GroupCode).HasColumnName("FN_GRP_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(f => f.UnitCode).HasColumnName("FN_UNT_COD").HasMaxLength(9);
        builder.Property(f => f.SerialNumber).HasColumnName("FN_SRL_NUM");
        builder.HasIndex(f => f.GroupCode).HasDatabaseName("IDX_FUNCTION_MAST_GRP_COD");
        builder.Ignore(f => f.DomainEvents);
    }
}

public class FunctionGroupConfiguration : IEntityTypeConfiguration<FunctionGroup>
{
    public void Configure(EntityTypeBuilder<FunctionGroup> builder)
    {
        builder.ToTable("FUNCTION_GROUP");
        builder.HasKey(g => g.GroupCode);
        builder.Property(g => g.GroupCode).HasColumnName("GR_GRP_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(g => g.GroupName).HasColumnName("GR_GRP_NAM").HasMaxLength(65).IsRequired();
        builder.Property(g => g.SerialNumber).HasColumnName("GR_SRL_NUM");
        builder.Ignore(g => g.DomainEvents);
    }
}

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.ToTable("GOAL_MAST");
        builder.HasKey(g => g.GoalCode);
        builder.Property(g => g.GoalCode).HasColumnName("GL_GOL_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(g => g.GoalName).HasColumnName("GL_GOL_NAM").HasMaxLength(65).IsRequired();
        builder.Ignore(g => g.DomainEvents);
    }
}

public class ModeConfiguration : IEntityTypeConfiguration<Mode>
{
    public void Configure(EntityTypeBuilder<Mode> builder)
    {
        builder.ToTable("MODE_MAST");
        builder.HasKey(m => m.ModeCode);
        builder.Property(m => m.ModeCode).HasColumnName("MD_MOD_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(m => m.ModeDescription).HasColumnName("MD_MOD_DES").HasMaxLength(65).IsRequired();
        builder.Ignore(m => m.DomainEvents);
    }
}

public class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.ToTable("SOURCE_MAST");
        builder.HasKey(s => s.SourceCode);
        builder.Property(s => s.SourceCode).HasColumnName("SR_SRC_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(s => s.SourceName).HasColumnName("SR_SRC_NAM").HasMaxLength(65).IsRequired();
        builder.Ignore(s => s.DomainEvents);
    }
}

public class SkillGroupConfiguration : IEntityTypeConfiguration<SkillGroup>
{
    public void Configure(EntityTypeBuilder<SkillGroup> builder)
    {
        builder.ToTable("SKILL_GROUP");
        builder.HasKey(sg => sg.GroupCode);
        builder.Property(sg => sg.GroupCode).HasColumnName("SK_GRP_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(sg => sg.GroupName).HasColumnName("SK_GRP_NAM").HasMaxLength(25).IsRequired();
        builder.Ignore(sg => sg.DomainEvents);
    }
}

public class TrainingGroupConfiguration : IEntityTypeConfiguration<TrainingGroup>
{
    public void Configure(EntityTypeBuilder<TrainingGroup> builder)
    {
        builder.ToTable("TRAIN_GROUP");
        builder.HasKey(tg => tg.GroupCode);
        builder.Property(tg => tg.GroupCode).HasColumnName("TR_GRP_COD").ValueGeneratedNever();
        builder.Property(tg => tg.GroupName).HasColumnName("TR_GRP_NAM").HasMaxLength(65);
        builder.Ignore(tg => tg.DomainEvents);
    }
}
