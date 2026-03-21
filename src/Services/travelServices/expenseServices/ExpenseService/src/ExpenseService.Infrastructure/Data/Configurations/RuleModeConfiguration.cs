using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class RuleModeConfiguration : IEntityTypeConfiguration<RuleMode>
{
    public void Configure(EntityTypeBuilder<RuleMode> builder)
    {
        builder.ToTable("RULE_MODE");
        builder.HasNoKey();

        builder.Property(e => e.UnitCode).HasColumnName("RL_COM_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.BandCode).HasColumnName("RL_BND_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.TravelType).HasColumnName("RL_TYP_COD").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.ModeCode).HasColumnName("RL_MOD_COD");
        builder.Property(e => e.ClassType).HasColumnName("RL_CLS_TYP").HasMaxLength(200);
        builder.Property(e => e.BudgetAmount).HasColumnName("RL_BUD_AMT").HasColumnType("decimal(19,0)");

        builder.Ignore(e => e.DomainEvents);
    }
}
