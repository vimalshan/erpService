using MasterService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterService.Infrastructure.Persistence.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("SKILL_MAST");
        builder.HasKey(s => s.SkillCode);
        builder.Property(s => s.SkillCode).HasColumnName("SK_SKL_COD").ValueGeneratedNever();
        builder.Property(s => s.SkillName).HasColumnName("SK_SKL_NAM").HasMaxLength(255).IsRequired();
        builder.Property(s => s.SkillType).HasColumnName("SK_SKL_TYP").HasMaxLength(1).IsRequired();
        builder.Property(s => s.WeightNum).HasColumnName("SK_WGT_NUM").HasColumnType("decimal(19,0)");
        builder.Property(s => s.Remark).HasColumnName("SK_SKL_REM").HasMaxLength(4000);
        builder.Property(s => s.EffectiveDate).HasColumnName("SK_EFF_DAT");
        builder.Property(s => s.CloseDate).HasColumnName("SK_CLS_DAT");
        builder.Ignore(s => s.DomainEvents);
        builder.Ignore(s => s.IsActive);
    }
}
