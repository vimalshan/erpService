using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class RuleDaConfiguration : IEntityTypeConfiguration<RuleDa>
{
    public void Configure(EntityTypeBuilder<RuleDa> builder)
    {
        builder.ToTable("RULE_DA");
        builder.HasNoKey();

        builder.Property(e => e.UnitCode).HasColumnName("RL_COM_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.GradeCode).HasColumnName("RL_BND_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.LocationGroup).HasColumnName("RL_LOC_GRP").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.TypeCode).HasColumnName("RL_TYP_COD").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.ArrangementSelf).HasColumnName("RL_ADM_SLF").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.CurrencyCode).HasColumnName("RL_CUR_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.DaType).HasColumnName("RL_DA_TYP").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.BudgetAmount).HasColumnName("RL_BUD_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.EffectiveDate).HasColumnName("RL_EFF_DAT");
        builder.Property(e => e.ClosureDate).HasColumnName("RL_CLS_DAT");

        builder.Ignore(e => e.DomainEvents);
    }
}
