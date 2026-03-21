using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class DaRuleConfiguration : IEntityTypeConfiguration<DaRule>
{
    public void Configure(EntityTypeBuilder<DaRule> builder)
    {
        builder.ToTable("DA_RULE");
        builder.HasKey(e => e.SerialNumber);

        builder.Property(e => e.SerialNumber).HasColumnName("RL_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.BandId).HasColumnName("RL_BND_ID");
        builder.Property(e => e.CountryCode).HasColumnName("RL_CTR_COD");
        builder.Property(e => e.SelfBookingFlag).HasColumnName("RL_SLF_FLG").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.CurrencyCode).HasColumnName("RL_CUR_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.BudgetAmount).HasColumnName("RL_BUD_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.EffectiveDate).HasColumnName("RL_EFF_DAT");
        builder.Property(e => e.ClosureDate).HasColumnName("RL_CLS_DAT");

        builder.Ignore(e => e.DomainEvents);
    }
}
