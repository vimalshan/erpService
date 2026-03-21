using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class RuleStayConfiguration : IEntityTypeConfiguration<RuleStay>
{
    public void Configure(EntityTypeBuilder<RuleStay> builder)
    {
        builder.ToTable("RULE_STAY");
        builder.HasNoKey();

        builder.Property(e => e.UnitCode).HasColumnName("RL_COM_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.BandCode).HasColumnName("RL_BND_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.StayType).HasColumnName("RL_STY_TYP");
        builder.Property(e => e.BudgetAmount).HasColumnName("RL_BUD_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.EffectiveDate).HasColumnName("RL_EFF_DAT");
        builder.Property(e => e.ClosureDate).HasColumnName("RL_CLS_DAT");

        builder.Ignore(e => e.DomainEvents);
    }
}
